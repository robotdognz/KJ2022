using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Together.Actors;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(AudioLowPassFilter))]
public class MusicController : MonoBehaviour
{
    private AudioSource Source;
    private AudioLowPassFilter Filter;

    [Range(0, 22000)] public float NormalCutoff = 22000;
    [Range(0, 22000)] public float ShadowCutoff = 2000;
    public float TransitionSpeed = 44000;

    private void Awake()
    {
        Source = GetComponent<AudioSource>();
        Filter = GetComponent<AudioLowPassFilter>();
    }

    private void Update()
    {
        Filter.cutoffFrequency = Mathf.MoveTowards(Filter.cutoffFrequency, PlayerController.PlayShadowMusic ? ShadowCutoff : NormalCutoff, Time.deltaTime * TransitionSpeed);
    }
}
