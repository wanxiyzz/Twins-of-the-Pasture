using UnityEngine;
namespace MyGame.PlantSystem
{
    public interface IHavested
    {
        public Vector3 Position
        {
            get;
        }
        public bool CanHarvest
        {
            get;
        }
        public void Harvested();
        public void Shoveled();
    }
}