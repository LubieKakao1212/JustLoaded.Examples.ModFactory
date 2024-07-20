using Custom2d_Engine.Rendering.Sprites;
using JustLoaded.Content;
using Microsoft.Xna.Framework;

namespace Template.Factory.Defs;

public class Item {
    
    public Color Color { get; }
    public DatabaseReference<Sprite> Sprite { get; }

    public Item(ContentKey key, Color? color = null) {
        this.Color = color.GetValueOrDefault(Color.White);
        this.Sprite = new DatabaseReference<Sprite>(key);
    }
}