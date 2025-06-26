using AkilliAlisverisApp.Models;
using Microsoft.Maui.Controls;

namespace AkilliAlisverisApp.Views;

public partial class TipDetailView : ContentPage
{
    public TipDetailView(Tip tip)
    {
        InitializeComponent();
        BindingContext = tip;
    }
}