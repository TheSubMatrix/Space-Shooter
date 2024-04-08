using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform _crosshairFar;
    [SerializeField] RectTransform _crosshairClose;
    [SerializeField] Image _healthBarMask;
    [SerializeField] LayerMask _raycastIgnoreMask;
    [SerializeField] GameObject _gameplayUI;
    [SerializeField] GameObject _gameOverUI;
    [SerializeField] TMP_Text _scoreTextHUD;
    [SerializeField] TMP_Text _scoreTextEndScreen;
    RectTransform _crosshairFarParent;
    RectTransform _crosshairCloseParent;
    AimingCrosshair _closeCrosshairColorUpdater;
    Canvas _canvas;
    Ship _ship;
    private void Start()
    {
        _ship = FindObjectOfType<Ship>();
        if(_crosshairFar != null && _crosshairFar.transform.parent.gameObject.HasComponent<RectTransform>())
        {
            _crosshairFarParent = _crosshairFar.parent.GetComponent<RectTransform>();
            if (_crosshairFar.GetComponentInParent<Canvas>())
            {
                _canvas = _crosshairFar.GetComponentInParent<Canvas>();
            }
        }
        if (_crosshairClose != null && _crosshairClose.transform.parent.gameObject.HasComponent<RectTransform>())
        {
            _crosshairCloseParent = _crosshairClose.parent.GetComponent<RectTransform>();
            if (_crosshairClose.gameObject.HasComponent<AimingCrosshair>())
            {
                _closeCrosshairColorUpdater = _crosshairClose.gameObject.GetComponent<AimingCrosshair>();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
            UpdateCrosshairLocations();
    }
    void UpdateCrosshairLocations()
    {
        if(_crosshairFar != null && _ship && _crosshairFarParent != null && _canvas != null)
        {
            Plane crosshairPlane = new Plane(Vector3.forward, new Vector3(0, 0, MeteorManager.MeteorSpawnZPosition));
            Ray playerForwardRay = new Ray(_ship.transform.position, _ship.transform.forward);
            crosshairPlane.Raycast(playerForwardRay, out float distance);
            Vector3 screenPositionOfCrosshair = Camera.main.WorldToScreenPoint(playerForwardRay.GetPoint(distance));
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_crosshairFarParent, screenPositionOfCrosshair, _canvas.worldCamera, out Vector2 anchoredPosition);
            _crosshairFar.anchoredPosition = anchoredPosition;
        }
        if (_crosshairClose != null && _ship && _crosshairCloseParent != null && _canvas != null)
        {
            Ray playerForwardRay = new Ray(_ship.transform.position, _ship.transform.forward);
            Vector3 screenPositionOfCrosshair;
            bool foundTargetInFrontOfPlayer = Physics.Raycast(playerForwardRay, out RaycastHit hitInfo, MeteorManager.MeteorSpawnZPosition, ~_raycastIgnoreMask);
            if (foundTargetInFrontOfPlayer)
            {
                screenPositionOfCrosshair = Camera.main.WorldToScreenPoint(hitInfo.point);
                if(_closeCrosshairColorUpdater != null)
                {
                    _closeCrosshairColorUpdater.Locked = true;
                }
            }
            else
            {
                Plane crosshairPlane = new Plane(Vector3.forward, new Vector3(0, 0, MeteorManager.MeteorSpawnZPosition / 8));
                crosshairPlane.Raycast(playerForwardRay, out float distance);
                screenPositionOfCrosshair = Camera.main.WorldToScreenPoint(playerForwardRay.GetPoint(distance));
                if (_closeCrosshairColorUpdater != null)
                {
                    _closeCrosshairColorUpdater.Locked = false;
                }
            }
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_crosshairCloseParent, screenPositionOfCrosshair, _canvas.worldCamera, out Vector2 anchoredPosition);
            _crosshairClose.anchoredPosition = anchoredPosition;
        }
    }
    public void UpdatePlayerHealthBar(uint health, uint maxHealth) 
    {
        if(_healthBarMask != null)
        {
            _healthBarMask.fillAmount = (float)health / (float)maxHealth;
        }
    }
    public void OnPlayerDeath()
    {
        if(_gameplayUI != null)
        {
            _gameplayUI.gameObject.SetActive(false);
        }
        if(_gameOverUI != null)
        {
            _gameOverUI.gameObject.SetActive(true);
        }
        if(_crosshairClose != null)
        {
            _crosshairClose.gameObject.SetActive(false);
        }
        if (_crosshairFar != null)
        {
            _crosshairFar.gameObject.SetActive(false);
        }
    }
    public void OnScoreUpdated(uint score)
    {
        if(_scoreTextHUD != null)
        {
            _scoreTextHUD.text = score.ToString();
        }
        if(_scoreTextEndScreen != null)
        {
            _scoreTextEndScreen.text = "Score: " + score.ToString();
        }
    }
    public void OnMenuButtonPressed()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void OnRetryButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
