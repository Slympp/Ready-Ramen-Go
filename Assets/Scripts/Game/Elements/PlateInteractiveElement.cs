using System.Collections.Generic;
using UnityEngine;

namespace Game.Elements {
    public class PlateInteractiveElement : InteractiveElement {
        public bool HasPlate { get; private set; }
        public bool HasBroth { get; private set; }
        public bool HasNoodles { get; private set; }
        public BrothType BrothType { get; private set; }
        public List<ToppingType> ToppingTypes { get; private set; } = new List<ToppingType>();
        
        [SerializeField] private Sprite plate;

        [SerializeField] private Sprite plateWithNoodles;
        
        [SerializeField] private Sprite plateWithDarkBroth;
        [SerializeField] private Sprite plateWithClearBroth;
        
        [SerializeField] private Sprite plateWithDarkBrothWithNoodles;
        [SerializeField] private Sprite plateWithClearBrothWithNoodles;

        public override void Receive(InteractiveElement element) {
            base.Receive(element);

            switch (element.GetElementType) {
                case ElementType.PlateStack:
                    Image.sprite = LastSprite = plate;
                    HasPlate = true;
                    HasContent = true;
                    break;

                case ElementType.Broth:
                    if (element is BrothInteractiveElement brothElement) {
                        BrothType = brothElement.GetBrothType;

                        Sprite brothSprite;
                        if (!HasNoodles)
                            brothSprite = BrothType == BrothType.Shio ? plateWithClearBroth : plateWithDarkBroth;
                        else
                            brothSprite = BrothType == BrothType.Shio
                                    ? plateWithClearBrothWithNoodles
                                    : plateWithDarkBrothWithNoodles;
                        
                        Image.sprite = LastSprite = brothSprite;

                        HasBroth = true;
                    }
                    break;
                
                case ElementType.Strainer:
                    Sprite noodlesSprite;
                    if (!HasBroth)
                        noodlesSprite = plateWithNoodles;
                    else
                        noodlesSprite = BrothType == BrothType.Shio
                                ? plateWithClearBrothWithNoodles
                                : plateWithDarkBrothWithNoodles;
                    
                    Image.sprite = LastSprite = noodlesSprite;
                    HasNoodles = true;
                    break;
                
                case ElementType.CuttingBoard:
                    if (element is CuttingBoardInteractiveElement cuttingBoard && !ToppingTypes.Contains(cuttingBoard.ToppingType)) {
                        ToppingTypes.Add(cuttingBoard.ToppingType);
                        Image.sprite = LastSprite = MergeSprites(Image.sprite, cuttingBoard.ToppingFinalSprite);
                    }
                    break;
            }
        }

        public override bool CanReceive(InteractiveElement element) {
            if (!CanInteractWith)
                return false;

            switch (element.GetElementType) {
                case ElementType.PlateStack:
                    if (HasPlate)
                        return false;
                    break;
                case ElementType.Broth:
                    if (HasBroth || !HasPlate)
                        return false;
                    break;
                case ElementType.Strainer:
                    if (HasNoodles || !HasPlate)
                        return false;
                    break;
                case ElementType.CuttingBoard:
                    if (!HasPlate || !HasBroth || !HasNoodles)
                        return false;
                    break;
                
                default:
                    return false;
            }

            return true;
        }

        public override void Empty() {
            base.Empty();

            HasPlate = HasBroth = HasNoodles = false;
            ToppingTypes.Clear();
        }
        
        private Sprite MergeSprites(Sprite back, Sprite front) {
            Vector2Int backSize = new Vector2Int((int)back.rect.width, (int)back.rect.height);
            Vector2Int frontSize = new Vector2Int((int)front.rect.width, (int)front.rect.height);
            
            Texture2D mergedTexture = new Texture2D(backSize.x, backSize.y);

            for (var x = 0; x < backSize.x; x++)
                for (var y = 0; y < backSize.y; y++)
                    mergedTexture.SetPixel(x, y, back.texture.GetPixel(x, y));

            for (int x = 0; x < frontSize.x; x++)
                for (int y = 0; y < frontSize.y; y++) {
                    var pixel = front.texture.GetPixel(x, y);
                    if (!pixel.a.Equals(0))
                        mergedTexture.SetPixel(x, y, pixel);
                }
            
            mergedTexture.Apply();
            return Sprite.Create(mergedTexture, new Rect(0, 0, mergedTexture.width, mergedTexture.height),
                Vector2.zero);
            }
    }
}