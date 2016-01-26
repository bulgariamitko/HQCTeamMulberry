namespace Poker.Interfaces
{
    using System.Windows.Forms;

    public interface IBot : ICharacter
    {
        int VerticalLocationCoordinate { get; set; }

        int HorizontalLocationCoordinate { get; set; }

        AnchorStyles VerticalLocation { get; set; }

        AnchorStyles HorizontalLocation { get; set; }

        AnchorStyles GetAnchorStyles();
    }
}
