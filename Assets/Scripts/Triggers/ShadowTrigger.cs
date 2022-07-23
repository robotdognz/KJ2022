using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Together.Levels
{
    public class ShadowTrigger : MonoBehaviour
    {
        public int CasterAmount;
        public float ShadowDistance = 10;

        private Vector2[] HitPositions
        {
            get
            {
                int TotalPointsHittingPlayer1 = 0;
                int TotalPointsHittingPlayer2 = 0;

                List<Vector2> Points = new List<Vector2>();

                float DegreesPerRay = 360f / CasterAmount;

                for (int I = 0; I < CasterAmount; I++)
                {
                    RaycastHit2D[] Hits = {};

                    Vector2 Dir = new Vector2(Mathf.Cos((DegreesPerRay * I) / (180 / Mathf.PI)), Mathf.Sin((DegreesPerRay * I) / (180 / Mathf.PI)));

                    Vector2 ShortestPoint = Vector2.zero;
                    RaycastHit2D ShortestHit = new RaycastHit2D();

                    foreach (RaycastHit2D H in Physics2D.RaycastAll(transform.position, Dir, ShadowDistance))
                    {
                        if (Vector2.Distance(H.point, transform.position) < Vector2.Distance(ShortestPoint, transform.position) || ShortestPoint == Vector2.zero)
                        {
                            ShortestPoint = H.point;
                            ShortestHit = H;
                        }
                    }

                    if (ShortestPoint != Vector2.zero)
                        Points.Add(ShortestPoint);

                    if (Application.isPlaying)
                    {
                        Actors.PlayerController P;

                        if (P = ShortestHit.collider.GetComponentInParent<Actors.PlayerController>())
                        {
                            if (ShortestHit.rigidbody == Actors.PlayerController.ActivePlayer)
                            {
                                TotalPointsHittingPlayer1++;
                            }
                            else if (ShortestHit.rigidbody == Actors.PlayerController.InactivePlayer)
                            {
                                TotalPointsHittingPlayer2++;
                            }
                        }

                        Actors.PlayerController.Instance.m_ActiveCharacter.IsInLight = TotalPointsHittingPlayer1 > 0;
                        Actors.PlayerController.Instance.m_InactiveCharacter.IsInLight = TotalPointsHittingPlayer2 > 0;
                    }
                }

                return Points.ToArray();
            }
        }

        private void Draw2DRay(Vector2 Start, Vector2 End)
        {
            Gizmos.DrawLine(new Vector3(Start.x, Start.y, transform.position.z), new Vector3(End.x, End.y, transform.position.z));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, ShadowDistance);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            foreach (Vector2 Point in HitPositions)
            {
                Draw2DRay(transform.position, Point);
            }
        }

        private void Update()
        {
            Vector2[] Points = HitPositions;
        }
    }
}