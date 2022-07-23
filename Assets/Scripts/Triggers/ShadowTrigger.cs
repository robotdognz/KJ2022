using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Together.Levels
{
    public class ShadowTrigger : MonoBehaviour
    {
        public int CasterAmount;
        public float ShadowDistance = 10;
        public string TargetTag;
        public bool DisableRaycastGizmos = false;

        private List<List<Vector2>> HitPositions
        {
            get
            {
                List<List<Vector2>> Points = new List<List<Vector2>>();

                int TotalPointsHittingPlayer1 = 0;
                int TotalPointsHittingPlayer2 = 0;

                foreach (GameObject G in GameObject.FindGameObjectsWithTag("ShadowCaster"))
                {
                    Transform Trans = G.transform;

                    for (int I = 0; I < CasterAmount; I++)
                    {
                        float Radians = ((360f / CasterAmount) * I) / (180 / Mathf.PI);
                        Vector2 Direction = new Vector2(Mathf.Cos(Radians), Mathf.Sin(Radians));

                        Vector2 ShortestPoint = Vector2.zero;
                        RaycastHit2D ShortestHit = new RaycastHit2D();

                        foreach (RaycastHit2D Hit in Physics2D.RaycastAll(Trans.position, Direction, ShadowDistance))
                        {
                            if ((Hit.collider.GetComponent<ShadowCaster2D>() && Hit.collider.GetComponent<ShadowCaster2D>().castsShadows && Hit.collider.GetComponent<ShadowCaster2D>().enabled) || Hit.collider.tag == "Player")
                            {
                                if (Vector2.Distance(Hit.point, Trans.position) < Vector2.Distance(ShortestPoint, Trans.position) || ShortestPoint == Vector2.zero)
                                {
                                    ShortestPoint = Hit.point;
                                    ShortestHit = Hit;
                                }
                            }
                        }

                        if (ShortestPoint != Vector2.zero)
                            Points.Add(new List<Vector2>() { Trans.position, ShortestPoint });

                        if (Application.isPlaying && ShortestHit.collider)
                        {
                            Actors.PlayerController P;

                            if (P = ShortestHit.collider.GetComponentInParent<Actors.PlayerController>())
                            {
                                if (ShortestHit.rigidbody == P.m_ActiveCharacter.CharacterObject)
                                {
                                    TotalPointsHittingPlayer1++;
                                }
                                else if (ShortestHit.rigidbody == P.m_InactiveCharacter.CharacterObject)
                                {
                                    TotalPointsHittingPlayer2++;
                                }
                            }
                        }
                    }
                }

                if (Application.isPlaying)
                {
                    Actors.PlayerController.Instance.m_ActiveCharacter.IsInLight = TotalPointsHittingPlayer1 > 0;
                    Actors.PlayerController.Instance.m_InactiveCharacter.IsInLight = TotalPointsHittingPlayer2 > 0;
                }

                return Points;
            }
        }

        private void Draw2DRay(Vector2 Start, Vector2 End)
        {
            Gizmos.DrawLine(new Vector3(Start.x, Start.y, transform.position.z), new Vector3(End.x, End.y, transform.position.z));
        }

        private void OnDrawGizmos()
        {
            foreach (List<Vector2> Point in HitPositions)
            {
                if (!DisableRaycastGizmos)
                {
                    Gizmos.color = Color.red;
                    Draw2DRay(Point[0], Point[1]);
                }

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(Point[0], ShadowDistance);
            }
        }

        private void OnDrawGizmosSelected()
        {
        }

        private void Update()
        {
            List<List<Vector2>> Points = HitPositions;
        }
    }
}