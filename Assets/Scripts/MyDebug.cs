using System.Collections.Generic;
using Game.Elements;

public static class MyDebug {
    public static string GetPlateContent(BrothType brothType, List<ToppingType> toppingTypes) {
        string debugToppings = "";
        foreach (var t in toppingTypes)
            debugToppings += t + " ";
        return $"Broth: {brothType} | Toppings: {debugToppings}";
    }
}