using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD1 : MonoBehaviour
{
    
    #region Inspector
    
    [SerializeField] 
    private List<GameObject> lives;

    [SerializeField] [Tooltip("Current Colors Player has")]
    private List<GameObject> colors;
    
    [SerializeField] [Tooltip("Time it takes lives to scale")]
    private float timeToScale = 1.5f;

    [SerializeField] private int livesOnStart = 3;
    
    #endregion
    
    private int _livesActive = 0;
    
    
    
    // Start is called before the first frame update
    void Start()
    {   
        
        // probably needs to be called on a different place in code, this is for now.
        InitLivesUI();
    }

    public void InitLivesUI()
    {
        for (int i = 0; i < livesOnStart; i++)
        {
            StartCoroutine(InstantiateLife(i, true));
            StartCoroutine(Wait(1));
        }

        _livesActive = livesOnStart;
    }

    public void AddLifeUI()
    {
        bool isOnMax = lives[lives.Count - 1].activeSelf;
        if (isOnMax)
        {
            Debug.Log("Player at max life! Not adding life");
            return;
        }
        
        StartCoroutine(InstantiateLife(_livesActive, true));
        _livesActive += 1;
    }

    public void RemoveLifeUI()
    {
        StartCoroutine(InstantiateLife(_livesActive - 1, true));
        _livesActive -= 1;
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
    
    IEnumerator InstantiateLife(int lifeIdx, bool state)
    {
        var scaleStart = state ? Vector3.zero : Vector3.one;
        var scaleEnd = state ? Vector3.one : Vector3.zero;
        float time = 0;
        
        if (state)
            lives[lifeIdx].SetActive(true);
        
        do
        {
            lives[lifeIdx].transform.localScale = Vector3.Lerp(scaleStart, scaleEnd, time);
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime / timeToScale;
        } while (time < 1);
        
        if (!state)
            lives[lifeIdx].SetActive(false);

    }
    
}
