using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class AuthorizationManager : MonoBehaviour
{
    [SerializeField] private InputField _loginInputField;
    [SerializeField] private InputField _passwordInputField;
    [SerializeField] private Text _errorText;
    public void Login()
    {
        if (InputFieldsNullOrEmpty()) return;
        StartCoroutine(EnterCoroutine());
    }
    private IEnumerator EnterCoroutine()
    {
        var request = UnityWebRequest.Get($"https://localhost:7075/Api/Authorization?login={_loginInputField.text}&password={_passwordInputField.text}");
        yield return request.SendWebRequest();

        var eUserEnter = JsonConvert.DeserializeObject<EUserEnter>(request.downloadHandler.text);

        if(eUserEnter == EUserEnter.Enter)
        {
            _errorText.text = "Вы вошли в систему";
        }
        else if (eUserEnter == EUserEnter.LoginNotHave)
        {
            _errorText.text = "Аккаунта не существуе. Может вы хотите создать аккаунт?";
        }
        else if (eUserEnter == EUserEnter.PasswordNotHave)
        {
            _errorText.text = "Пароль неверен";
        }
    }
    private IEnumerator RegisterCoroutine()
    {
        var request = UnityWebRequest.Get($"https://localhost:7075/Api/Register?login={_loginInputField.text}&password={_passwordInputField.text}");
        yield return request.SendWebRequest();

        var eUserEnter = JsonConvert.DeserializeObject<EUserAdd>(request.downloadHandler.text);

        if (eUserEnter == EUserAdd.LoginExists)
        {
            _errorText.text = "Такой логин уже сущесвует";
        }
        else if (eUserEnter == EUserAdd.UserCreated)
        {
            _errorText.text = "Пользователь создан";
        }
    }
    private bool InputFieldsNullOrEmpty()
    {
        if (string.IsNullOrEmpty(_loginInputField.text))
        {
            _errorText.text = "Логин не может быть пустым!!";
            return true;
        }
        else if (string.IsNullOrEmpty(_passwordInputField.text))
        {
            _errorText.text = "Пароль не может быть пустым!!";
            return true;
        }
        return false;
    }
    public void Register()
    {
        if (InputFieldsNullOrEmpty()) return;
        StartCoroutine(RegisterCoroutine());
    }
    public enum EUserEnter
    {
        Enter = 0,
        LoginNotHave = 1,
        PasswordNotHave = 2,
    }
    public enum EUserAdd
    {
        LoginExists = 1,
        UserCreated = 2,
    }
}
