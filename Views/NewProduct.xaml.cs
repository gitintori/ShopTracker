using ShopTracker.Models;

namespace ShopTracker.Views;

public partial class NewProduct : ContentPage
{
    public NewProduct()
    {
        InitializeComponent();
    }

    private async void Add_Product_Clicked(object sender, EventArgs e)
    {
        try
        {
            Product p = new Product
            {
                Description = txt_description.Text,
                Quantity = Convert.ToDouble(txt_quantity.Text),
                Price = Convert.ToDouble(txt_price.Text),
                Category = categoryPicker.SelectedItem?.ToString() ?? "No Category"
            };

            await App.Db.Insert(p);
            await DisplayAlert("Sucess!", "Product added.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void PickCategory(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1 && BindingContext is Product product)
        {
            product.Category = picker.Items[selectedIndex];
        }
    }
}