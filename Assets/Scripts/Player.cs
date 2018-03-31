using UnityEngine;
using System.Collections;
using DigitalRubyShared;

public class Player : MonoBehaviour
{
    public SpawnManager spawnManager;
    public OneSkill oneSkill;
    public TwoSkill twoSkill;
    public SwipeSkill swipeSkill;
    public float nukeSpawnDelay = 0.5f;
    public float tapRadius = 3.0f;
    public float twofingerRadius = 6.0f;
    public float coolDownTwoSkill = 3.0f;
    public float coolDownThreeSkill = 10.0f;

    [HideInInspector]
    public float gameTimer = 0;
    protected float bigAoeTimer = 0.0f;
    protected float nukeTimer = 0.0f;
    protected bool bigAoe = true;

    protected bool nuke = true;

    private TapGestureRecognizer oneFingerTapGesture;
    private TapGestureRecognizer twoFingerTapGesture;
    private TapGestureRecognizer threeFingerTapGesture;
    private SwipeGestureRecognizer swipeGesture;

    // Use this for initialization
    void Start()
    {
        swipeGesture = new SwipeGestureRecognizer();
        swipeGesture.StateUpdated += SwipeSkill;
        swipeGesture.EndImmediately = true;
        swipeGesture.Direction = SwipeGestureRecognizerDirection.Up;
        oneFingerTapGesture = new TapGestureRecognizer();
        oneFingerTapGesture.MinimumNumberOfTouchesToTrack = oneFingerTapGesture.MaximumNumberOfTouchesToTrack = 1;
        oneFingerTapGesture.StateUpdated += OneFingerTap;
        twoFingerTapGesture = new TapGestureRecognizer();
        twoFingerTapGesture.MinimumNumberOfTouchesToTrack = twoFingerTapGesture.MaximumNumberOfTouchesToTrack = 2;
        twoFingerTapGesture.StateUpdated += TwoFingerTap;
        threeFingerTapGesture = new TapGestureRecognizer();
        threeFingerTapGesture.MinimumNumberOfTouchesToTrack = threeFingerTapGesture.MaximumNumberOfTouchesToTrack = 3;
        threeFingerTapGesture.StateUpdated += ThreeFingerTap;
        twoFingerTapGesture.RequireGestureRecognizerToFail = oneFingerTapGesture;
        threeFingerTapGesture.RequireGestureRecognizerToFail = twoFingerTapGesture;
        FingersScript.Instance.AddGesture(oneFingerTapGesture);
        FingersScript.Instance.AddGesture(twoFingerTapGesture);
        FingersScript.Instance.AddGesture(threeFingerTapGesture);
        FingersScript.Instance.AddGesture(swipeGesture);
    }

    //private Collider2D[] GestureIntersectsSprite(GestureRecognizer g, float radius)
    //{
    //    Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(g.FocusX, g.FocusY, -Camera.main.transform.position.z));
    //    Collider2D[] cols = Physics2D.OverlapCircleAll(worldPos, radius);///(worldPos);
    //    return cols;
    //    //return (col != null && col.gameObject != null && col.gameObject == obj);
    //}


    private void SwipeSkill(GestureRecognizer gesture)
    {
        //Debug.LogFormat("Swipe state: {0}", gesture.State);

        //SwipeGestureRecognizer swipe = gesture as SwipeGestureRecognizer;
        if (gesture.State == GestureRecognizerState.Ended)
        {
            //Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(gesture.FocusX, gesture.FocusY, -Camera.main.transform.position.z));
            swipeSkill.ActivateSkill(Vector3.zero);
        }
    }

    private void OneFingerTap(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(gesture.FocusX, gesture.FocusY, -Camera.main.transform.position.z));
            oneSkill.ActivateSkill(worldPos);
        }
    }

    private void TwoFingerTap(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            if (bigAoe)
            {
                bigAoe = false;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(gesture.FocusX, gesture.FocusY, -Camera.main.transform.position.z));
                twoSkill.ActivateSkill(worldPos);

                //foreach (Collider2D col in GestureIntersectsSprite(gesture, twofingerRadius))
                //{
                //    DefaultEnemy enemy = col.gameObject.GetComponent<DefaultEnemy>();
                //    if (enemy)
                //    {
                //        enemy.TakeDamage();
                //    }
                //}
            }
        }
    }

    private void ThreeFingerTap(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            if (nuke)
            {
                nuke = false;
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (obj && obj.GetComponent<DefaultEnemy>())
                    {
                        obj.GetComponent<DefaultEnemy>().Death();
                    }
                }
                StartCoroutine(DelaySpawn());
            }
        }
    }

    IEnumerator DelaySpawn()
    {
        spawnManager.nuking = true;
        yield return new WaitForSeconds(nukeSpawnDelay);
        spawnManager.nuking = false;
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer += Time.deltaTime;
        if(!bigAoe)
        {
            bigAoeTimer += Time.deltaTime;
            if(bigAoeTimer >= coolDownTwoSkill)
            {
                bigAoeTimer = 0;
                bigAoe = true;
            }
        }

        if (!nuke)
        {
            nukeTimer += Time.deltaTime;
            if (nukeTimer >= coolDownThreeSkill)
            {
                nukeTimer = 0;
                nuke = true;
            }
        }
    }
}
