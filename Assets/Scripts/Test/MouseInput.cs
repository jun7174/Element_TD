using UnityEngine;

namespace ElementTD
{
    public class MouseInput : MonoBehaviour
    {
        Vector3 MousePostition;
        public LayerMask whatisPlatform;
        
        [SerializeField]
        TileMapManager tileMapManager;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(MousePostition, 0.2f);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                MousePostition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D overCollider2d = Physics2D.OverlapCircle(MousePostition, 0.01f, whatisPlatform);
                Debug.Log(MousePostition);
                if (overCollider2d != null)
                {
                    Debug.Log("타일감지");
                    tileMapManager.SpriteSet(MousePostition);
                    //overCollider2d.transform.GetComponent<Bricks>().MakeDot(MousePostition);
                }
                else
                {
                    Debug.Log("타일 미감지");
                }
            }
        }
    }
}