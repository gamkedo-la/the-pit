using UnityEngine;
using Variables;

namespace Player
{
    public class PlayerRespawnController : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObjectVariable player;

        private PlayerSpawnPoint spawnPoint;

        private void Start()
        {
            Respawn();
        }

        public void SetSpawnPoint(PlayerSpawnPoint sp)
        {
            spawnPoint = sp;
        }

        public void Respawn()
        {
            if (player.Value != null) Destroy(player.Value);
            var pos = transform.position;
            if (spawnPoint != null) pos = spawnPoint.transform.position;

            player.Value = Instantiate(playerPrefab, pos, Quaternion.identity);
        }
    }
}