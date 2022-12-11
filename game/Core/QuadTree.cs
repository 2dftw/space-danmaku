using System.Diagnostics;
using game.Core.Entities;
using game.general;

namespace game.Core;
// TODO: tests
public class QuadTree
{
    public QuadTree(Vec2 position, Vec2 size, int currentCurrentLevel, int maxLevel = 1)
    {
        this.position = position;
        this.size = size;
        currentLevel = currentCurrentLevel;
        this.maxLevel = maxLevel;
        entities = new List<Entity>();

        if (currentLevel == this.maxLevel)
        {
            upperLeft = null;
            bottomLeft = null;
            upperRight = null;
            bottomRight = null;
        }
        else
        {
            var upperLeftPosition = this.position + new Vec2(-this.size.X / 2, this.size.Y / 2);
            upperLeft = new QuadTree(upperLeftPosition, this.size / 2, currentLevel + 1, this.maxLevel);

            var bottomLeftPosition = this.position + new Vec2(-this.size.X / 2, -this.size.Y / 2);
            bottomLeft = new QuadTree(bottomLeftPosition, this.size / 2, currentLevel + 1, this.maxLevel);

            var upperRightPosition = this.position + new Vec2(this.size.X / 2, this.size.Y / 2);
            upperRight = new QuadTree(upperRightPosition, this.size / 2, currentLevel + 1, this.maxLevel);

            var bottomRightPosition = this.position + new Vec2(this.size.X / 2, -this.size.Y / 2);
            bottomRight = new QuadTree(bottomRightPosition, this.size / 2, currentLevel + 1, this.maxLevel);
        }
    }

    private readonly Vec2 position;      // Center position
    private readonly Vec2 size;          // Bounds of the space
    private readonly int currentLevel;
    private readonly int maxLevel;

    private readonly QuadTree? upperLeft;
    private readonly QuadTree? bottomLeft;
    private readonly QuadTree? upperRight;
    private readonly QuadTree? bottomRight;

    private readonly List<Entity> entities;
    
    public void Add(Entity entity)
    {
        if (currentLevel == maxLevel)
        {
            entities.Add(entity);
        }
        else
        {
            if (entity.Position.X <= position.X && entity.Position.Y <= position.Y)
                upperLeft!.Add(entity);
            else if (entity.Position.X <= position.X && entity.Position.Y > position.Y)
                bottomLeft!.Add(entity);
            else if (entity.Position.X > position.X && entity.Position.Y <= position.Y)
                upperRight!.Add(entity);
            else if (entity.Position.X > position.X && entity.Position.Y > position.Y)
                upperLeft!.Add(entity);
        }
    }

    public List<Entity> GetClosest(Entity entity)
    {
        if (currentLevel == maxLevel)
            return entities;
        
        if (entity.Position.X <= position.X && entity.Position.Y <= position.Y)
            return upperLeft!.GetClosest(entity);
        if (entity.Position.X <= position.X && entity.Position.Y > position.Y)
            return bottomLeft!.GetClosest(entity);
        if (entity.Position.X > position.X && entity.Position.Y <= position.Y)
            return upperRight!.GetClosest(entity);
        if (entity.Position.X > position.X && entity.Position.Y > position.Y)
            return upperLeft!.GetClosest(entity);

        throw new UnreachableException();
    }

    public void ClearRecursively()
    {
        entities.Clear();
        if (currentLevel < maxLevel)
        {
            upperLeft!.ClearRecursively();
            bottomLeft!.ClearRecursively();
            upperRight!.ClearRecursively();
            bottomRight!.ClearRecursively();
        }
    }
}