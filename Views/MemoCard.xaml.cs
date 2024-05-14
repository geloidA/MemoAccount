using MemoAccount.Models;

namespace MemoAccount.Views;


/// <summary>
/// Визуализация заметки.
/// </summary>
public partial class MemoCard
{
    /// <summary>
    /// Заметка, отображаемая в карточке.
    /// </summary>
    public static readonly DependencyProperty MemoProperty =
        DependencyProperty.Register(nameof(Memo), typeof(Memo), typeof(MemoCard));

    /// <summary>
    /// Заметка, отображаемая в карточке.
    /// </summary>
    public Memo Memo
    {
        get => (Memo)GetValue(MemoProperty);
        set => SetValue(MemoProperty, value);
    }

    /// <summary>
    /// Инициализация элемента управления.
    /// </summary>
    public MemoCard()
    {
        InitializeComponent();
    }
}