using Custom2d_Engine.Rendering;
using Custom2d_Engine.Rendering.Sprites;
using Custom2d_Engine.Rendering.Sprites.Atlas;
using Custom2d_Engine.TMX;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Template.Factory.Loading;

public class SpriteLoadingContext {

    private readonly SpriteAtlas<Color> _atlas;
    private readonly SpriteAtlasLoader<Color> _atlasLoader;
    private readonly TMXLoader<Color> _tmxLoader;

    public SpriteLoadingContext(ContentManager content, SpriteAtlas<Color> atlas, params string[] textureNames) {
        _atlas = atlas;
        _atlasLoader = new SpriteAtlasLoader<Color>(content, _atlas, textureNames);
        _tmxLoader = new TMXLoader<Color>(content, _atlasLoader);
    }

    public Sprite LoadSprite(string path) {
        return _atlasLoader.Load(path)[0];
    }

    public LoadedMap<Color> LoadTmxMap(string path) {
        return _tmxLoader.LoadedMap(path);
    }

    public void Finish(RenderPipeline renderPipeline) {
        _atlas.Compact();
        
        var textures = _atlas.AtlasTextures;
        renderPipeline.SetLitAtlases(textures[0], textures[1], textures[2]);
    }
    
}