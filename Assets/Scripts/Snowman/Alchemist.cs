using System.Collections;
using Snowman.Skills;
using UnityEngine;
using Utilities;

namespace Snowman
{
    public class Alchemist : BaseSnowman
    {
        public GameObject alchemyVfx;
        public float maxDistance;
        public float timer;

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(PlaceAlchemy());
        }

        private IEnumerator PlaceAlchemy()
        {
            while (health > 0)
            {
                var posX = Random.Range(-maxDistance, maxDistance) + transform.position.x;
                var posZ = Random.Range(-maxDistance, maxDistance) + transform.position.z;
                var position = new Vector3(posX, 1, posZ);
                var alchemy = Instantiate(alchemyVfx, position, Quaternion.identity);
                alchemy.GetComponent<Alchemy>().SetAlchemy(level == SnowmanLevel.Advanced, MySnowmanSO.attack, MySnowmanSO.shieldBreakEfficiency);
                yield return new WaitForSeconds(MySnowmanSO.attackSpeed);
            }
            
            yield return null;
        }
    }
}
