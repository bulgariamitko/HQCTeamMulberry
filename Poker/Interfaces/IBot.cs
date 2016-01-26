using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
