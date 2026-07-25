using UnityEngine;
using UnityEngine.Tilemaps;

namespace ElementTD
{
    [CreateAssetMenu(fileName = "NewCustomeTile", menuName = "ElementTD/Tile")]
    public class CustomTileBase : RuleTile
    {
        [SerializeField]
        ElementType elementType;

        public ElementType GetElementType()
        {
            return elementType;
        }
    }
}