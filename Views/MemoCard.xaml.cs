using MemoAccount.Models;

namespace MemoAccount.Views;

/// <summary>
/// Interaction logic for MemoCard.xaml
/// </summary>
public partial class MemoCard
{
    public static readonly DependencyProperty MemoProperty =
        DependencyProperty.Register(nameof(Memo), typeof(Memo), typeof(MemoCard));

    public Memo Memo
    {
        get => (Memo)GetValue(MemoProperty);
        set => SetValue(MemoProperty, value);
    }

    public MemoCard()
    {
        InitializeComponent();
    }
}