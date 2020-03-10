using UnityEngine.UI;

namespace ChessBoard
{
    public interface ICellAbility
    {
        void On();
        void Off();
    }

    public class CellOutlineAbility : ICellAbility
    {
        private readonly Image _outlineImage;

        public CellOutlineAbility(Image outlineImage)
        {
            _outlineImage = outlineImage;
        }

        public void On()
        {
            _outlineImage.enabled = true;
        }

        public void Off()
        {
            _outlineImage.enabled = false;
        }
    }
}