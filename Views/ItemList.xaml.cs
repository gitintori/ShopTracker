using ShopTracker.Models;
using System.Collections.ObjectModel;

namespace ShopTracker.Views;

public partial class ItemList : ContentPage
{
    ObservableCollection<Product> list = new ObservableCollection<Product>();

    private Product _selectedProduct;

    public ItemList()
    {
        InitializeComponent();

        lst_products.ItemsSource = list;
    }

    protected async override void OnAppearing()
    {
        try
        {
            base.OnAppearing();

            categoryPicker.SelectedIndex = 0;

            list.Clear();

            List<Product> tmp = await App.Db.GetAll();

            tmp.ForEach(i => list.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void Add_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NewProduct());

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;

            lst_products.IsRefreshing = true;

            list.Clear();

            List<Product> tmp = await App.Db.Search(q);

            tmp.ForEach(i => list.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_products.IsRefreshing = false;
        }
    }

    private async void PickCategory(object sender, EventArgs e)
    {

        try
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex == -1)
                return;

            if (picker == null) return;

            string selectedCategory = picker.Items[selectedIndex];

            lst_products.IsRefreshing = true;
            list.Clear();

            List<Product> tmp;

            if (selectedCategory == "All")
            {
                tmp = await App.Db.GetAll();
            }
            else
            {
                tmp = await App.Db.FilterByCategory(selectedCategory);
            }

            if (!tmp.Any() && selectedCategory != "All")
            {
                await DisplayAlert("Warning", $"No items found in '{selectedCategory}'.", "OK");
                picker.SelectedIndex = 0;
                tmp = await App.Db.GetAll();
            }

            tmp.ForEach(i => list.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_products.IsRefreshing = false;
        }
    }

    private void Sum_Clicked(object sender, EventArgs e)
    {
        double sum = list.Sum(i => i.Total);

        string msg = $"Total: {sum:C}";

        DisplayAlert("Total:", msg, "OK");
    }

    private void Edit_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (_selectedProduct == null)
            {
                DisplayAlert("Warning", "Please select an item.", "OK");
                return;
            }

            Navigation.PushAsync(new Views.EditItem
            {
                BindingContext = _selectedProduct,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }
    private async void Delete_Clicked(object sender, EventArgs e)
    {
        if (_selectedProduct == null)
        {
            await DisplayAlert("Ops", "No items found.", "OK");
            return;
        }

        bool confirm = await DisplayAlert("Confirm", $"Do you want to remove the product {_selectedProduct.Description}?", "Yes", "No");

        if (confirm)
        {
            try
            {
                await App.Db.Delete(_selectedProduct.Id);

                list.Remove(_selectedProduct);

                await DisplayAlert("Sucess", "Item removed.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }


    private void lst_products_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            _selectedProduct = e.SelectedItem as Product;

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_products_Refreshing(object sender, EventArgs e)
    {
        try
        {
            list.Clear();

            List<Product> tmp = await App.Db.GetAll();

            tmp.ForEach(i => list.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_products.IsRefreshing = false;
        }
    }

}