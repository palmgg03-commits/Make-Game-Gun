using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 20f;      // ¤ÇÒÁàÃçÇã¹¡ÒÃ¾Øè§¢Í§¡ÃÐÊØ¹
    [SerializeField] private float damage = 25f;     // ´ÒàÁ¨·Õè·ÓµèÍ Em
    [SerializeField] private float lifeTime = 3f;    // ÃÐÂÐàÇÅÒ·ÓÅÒÂµÑÇàÍ§ËÒ¡ÂÔ§äÁèâ´¹ÍÐäÃàÅÂ

    private void Start()
    {
        // ·ÓÅÒÂ¡ÃÐÊØ¹µÑÇàÍ§ÍÑµâ¹ÁÑµÔËÅÑ§¼èÒ¹ä» 3 ÇÔ¹Ò·Õ (»éÍ§¡Ñ¹Ã¡áÃÁ/«Õ¹)
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // ãËé¡ÃÐÊØ¹¾Øè§ä»¢éÒ§Ë¹éÒ (µÒÁá¡¹ Z ¢Í§µÑÇÁÑ¹àÍ§) ´éÇÂ¤ÇÒÁàÃçÇ·Õè¡ÓË¹´
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // »éÍ§¡Ñ¹¡ÃÐÊØ¹ª¹¡ÑºµÑÇ Player àÍ§
        if (other.CompareTag("Player")) return;

        // µÃÇ¨ÊÍºÇèÒª¹¡ÑºÇÑµ¶Ø·ÕèÁÕàÅ×Í´ (IDamageable) àªè¹ Em ËÃ×ÍäÁè
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage); // ÊÃéÒ§´ÒàÁ¨ãËé Em
        }

        // àÁ×èÍª¹¡ÑºÍÐäÃ¡çµÒÁ (¾×é¹, ¡Óá¾§, ËÃ×Í Em) ãËé·ÓÅÒÂ¡ÃÐÊØ¹·Ôé§·Ñ¹·Õ
        Destroy(gameObject);
    }
} 
//Hello world from p'earth
