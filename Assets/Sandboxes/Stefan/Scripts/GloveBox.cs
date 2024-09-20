using UnityEngine;

[RequireComponent(typeof(GrabbableItem))]
public class GloveBox : MonoBehaviour
{
    [SerializeField] Transform _renderer;
    [SerializeField] Transform _openRot;
    [SerializeField] Transform _closeRot;

    [SerializeField] Transform _spawnSpots;

    [SerializeField] GameObject[] _itemsRandomPool;
    [SerializeField] SoundName _sound1;
    [SerializeField] SoundName _sound2;

    GrabbableItem _grabbableItem;

    bool _isOpen;

    void Awake()
    {
        _renderer.parent = null;
        _renderer.parent = _closeRot;

        _grabbableItem = GetComponent<GrabbableItem>();
        _grabbableItem.onPlayerInteract.AddListener(controller =>
        {
            _isOpen = !_isOpen;
            if(_isOpen)
            {
                SoundManager.Instance.PlaySound(_sound1);
                _renderer.parent = null;
                _renderer.parent = _openRot;
                _renderer.localRotation = Quaternion.identity;

                foreach (Transform spot in _spawnSpots)
                {
                    if(spot.childCount > 0)
                    Destroy(spot.GetChild(0).gameObject);
                    var item = Instantiate(_itemsRandomPool[Random.Range(0, _itemsRandomPool.Length)], spot);
                }
            }
            else
            {
                _renderer.parent = null;
                _renderer.parent = _closeRot;
                _renderer.localRotation = Quaternion.identity;
                SoundManager.Instance.PlaySound(_sound2);

            }
        });
    }


}
