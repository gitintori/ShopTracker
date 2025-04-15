using ShopTracker.Models;

namespace ShopTracker.Views;

public partial class EditItem : ContentPage
{
    public EditItem()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is Product product)
        {
            txt_description.Text = product.Description;
            txt_quantity.Text = product.Quantity.ToString();
            txt_price.Text = product.Price.ToString();

            if (!string.IsNullOrEmpty(product.Category))
            {
                int index = categoryPicker.Items.IndexOf(product.Category);
                categoryPicker.SelectedIndex = index != -1 ? index : 0;
            }
        }
    }

    private async void Save_Edition_Clicked(object sender, EventArgs e)
    {
        try
        {
            Product attached_product = BindingContext as Product;

            Product p = new Product
            {
                Id = attached_product.Id,
                Description = txt_description.Text,
                Quantity = Convert.ToDouble(txt_quantity.Text),
                Price = Convert.ToDouble(txt_price.Text),
                Category = attached_product.Category
            };

            await App.Db.Update(p);
            await DisplayAlert("Sucess", "Product updated.", "OK");
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