using System;
using System.Collections.Generic;
using Game.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Manager {
    public class OutlineManager : MonoBehaviour {
        [ColorUsage(true,true)]
        [SerializeField] private Color hoverColor;

        [ColorUsage(true,true)]
        [SerializeField] private Color targetColor;

        [SerializeField] private List<InteractiveElement> outlinedElements;
        
        private Dictionary<ElementType, List<Tuple<InteractiveElement, Material>>> _materialDictionary = 
                new Dictionary<ElementType, List<Tuple<InteractiveElement, Material>>>();

        void Awake() {
            foreach (var e in outlinedElements) {
                var list = new List<Tuple<InteractiveElement, Material>>();
                
                if (_materialDictionary.TryGetValue(e.GetElementType, out var l))
                    list = l;
                else
                    _materialDictionary.Add(e.GetElementType, list);
                
                var image = e.GetComponent<Image>();
                if (image != null) {
                    image.material = CloneMaterial(image);
                    list.Add(new Tuple<InteractiveElement, Material>(e, image.material));
                }
            }
        }

        public void ToggleHoverOutline(InteractiveElement element = null) {
            foreach (var item in _materialDictionary) {
                foreach (var i in item.Value) {
                    var toHover = i.Item1 == element && element.CanBeSelected();
                    i.Item2.SetColor(Constants.ColorShaderProperty, toHover ? hoverColor : Color.clear);
                    i.Item2.SetFloat(Constants.EnabledShaderProperty, toHover ? 1f : 0f);
                }
            }
        }

        public void ToggleTargetOutline(ElementType elementType, InteractiveElement selection) {
            foreach (var item in _materialDictionary) {
                foreach (var i in item.Value) {

                    var canReceive = selection != null && i.Item1.CanReceive(selection);
                    i.Item2.SetColor(Constants.ColorShaderProperty, targetColor);
                    i.Item2.SetFloat(Constants.EnabledShaderProperty, 
                        (item.Key == elementType || (item.Key == ElementType.TrashBin)) && canReceive ? 1f : 0f);
                }
            }
        }

        public void AddMaterialList(ElementType type, List<Tuple<InteractiveElement, Image>> list) {
            var newList = new List<Tuple<InteractiveElement, Material>>();
            foreach (var item in list) {
                var clonedMaterial = CloneMaterial(item.Item2);
                
                item.Item2.material = clonedMaterial;
                newList.Add(new Tuple<InteractiveElement, Material>(item.Item1, clonedMaterial));
            }
            _materialDictionary.Add(type, newList);
        }
        
        private Material CloneMaterial(Image image) {
            var mat = Instantiate(image.material);
            mat.CopyPropertiesFromMaterial(image.material);
            return mat;
        }
    }
}
