using System.Net.Http.Headers;
using System.Net.Http.Json;

using System.Net;
using System.Text.Json;

/// <summary>
/// Помощник Http-клиента
/// </summary>
public class HttpClientHelper
{
  /// <summary>
  /// Получить данные с сервера
  /// </summary>
  /// <param name="httpClient">Http-клиент</param>
  /// <param name="endpoint">Адрес запроса</param>
  /// <param name="errorMessage">Сообщение об ошибке</param>
  /// <typeparam name="T">Тип возвращаемых данных</typeparam>
  /// <returns>Данные</returns>
  public static async Task<T?> GetJsonFromServer<T>(HttpClient httpClient, string endpoint, string errorMessage)
    where T : class, new()
  {
    var task = new Task(() => Console.Error.WriteLine(errorMessage));
    return await GetJsonFromServer<T>(httpClient, endpoint, task);
  }

  /// <summary>
  /// Получить данные с сервера
  /// </summary>
  /// <param name="httpClient">Http-клиент</param>
  /// <param name="endpoint">Адрес запроса</param>
  /// <param name="errorTask">Действие при возникновении ошибки</param>
  /// <typeparam name="T">Тип возвращаемых данных</typeparam>
  /// <returns>Данные</returns>
  public static async Task<T?> GetJsonFromServer<T>(HttpClient httpClient, string endpoint, Task errorTask)
    where T : class, new()
  {
    ArgumentNullException.ThrowIfNull(httpClient);
    ArgumentNullException.ThrowIfNull(endpoint);

    try
    {
      using var httpResponse = await httpClient.GetAsync(endpoint);

      return httpResponse.StatusCode == HttpStatusCode.OK
        ? await httpResponse.Content.ReadFromJsonAsync<T>()
        : new T();
    }
    catch (Exception e)
    {
      errorTask.Start();
      await errorTask;
      WriteException(e, endpoint);
    }

    return null;
  }

  /// <summary>
  /// Отправить данные на http-сервер
  /// </summary>
  /// <param name="httpClient">http-клиент</param>
  /// <param name="endpoint">эндпоинт</param>
  /// <param name="body">Тело запроса</param>
  /// <param name="errorMessage">Сообщение об ошибке</param>
  public static async Task PostJsonToServer(HttpClient httpClient, string endpoint, object? body, string errorMessage)
  {
    var task = new Task(() => Console.Error.WriteLineAsync(errorMessage));
    await PostJsonToServer(httpClient, endpoint, body, task);
  }

  /// <summary>
  /// Отправить данные на http-сервер
  /// </summary>
  /// <param name="httpClient">http-клиент</param>
  /// <param name="endpoint">эндпоинт</param>
  /// <param name="body">Тело запроса</param>
  /// <param name="errorAction">Действие при возникновении ошибки</param>
  public static async Task PostJsonToServer(HttpClient httpClient, string endpoint, object? body, Task errorAction)
  {
    ArgumentNullException.ThrowIfNull(httpClient);
    ArgumentNullException.ThrowIfNull(endpoint);

    try
    {
      var httpContent = new StringContent(body == null ? string.Empty : JsonSerializer.Serialize(body));
      httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      var response = await httpClient.PostAsync(endpoint, httpContent);
      response.EnsureSuccessStatusCode();

      if (response.StatusCode is HttpStatusCode.NoContent) return;

      errorAction.Start();
      await errorAction;
    }
    catch (Exception e)
    {
      errorAction.Start();
      await errorAction;
      WriteException(e, endpoint);
    }
  }

  /// <summary>
  /// Отправить данные на частичное изменение на http-сервер
  /// </summary>
  /// <param name="httpClient">http-клиент</param>
  /// <param name="endpoint">эндпоинт</param>
  /// <param name="body">Тело запроса</param>
  /// <param name="errorMessage">Сообщение об ошибке</param>
  public static async Task PatchJsonToServer(HttpClient httpClient, string endpoint, object? body, string errorMessage)
  {
    var task = new Task(() => Console.Error.WriteLine(errorMessage));
    await PatchJsonToServer(httpClient, endpoint, body, task);
  }

  /// <summary>
  /// Отправить данные на частичное изменение на http-сервер
  /// </summary>
  /// <param name="httpClient">http-клиент</param>
  /// <param name="endpoint">эндпоинт</param>
  /// <param name="body">Тело запроса</param>
  /// <param name="errorAction">Действие при возникновении ошибки</param>
  public static async Task PatchJsonToServer(HttpClient httpClient, string endpoint, object? body, Task errorAction)
  {
    ArgumentNullException.ThrowIfNull(httpClient);
    ArgumentNullException.ThrowIfNull(endpoint);

    try
    {
      var httpContent = new StringContent(body == null ? string.Empty : JsonSerializer.Serialize(body));
      httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      var response = await httpClient.PatchAsync(endpoint, httpContent);
      response.EnsureSuccessStatusCode();

      if (response.StatusCode is HttpStatusCode.NoContent) return;

      errorAction.Start();
      await errorAction;
    }
    catch (Exception e)
    {
      errorAction.Start();
      await errorAction;
      WriteException(e, endpoint);
    }
  }

  /// <summary>
  /// Отправить данные для полного обновления на http-сервер
  /// </summary>
  /// <param name="httpClient">http-клиент</param>
  /// <param name="endpoint">эндпоинт</param>
  /// <param name="body">Тело запроса</param>
  /// <param name="errorMessage">Сообщение об ошибке</param>
  public static async Task PutJsonToServer(HttpClient httpClient, string endpoint, object? body, string errorMessage)
  {
    var task = new Task(() => Console.Error.WriteLine(errorMessage));
    await PutJsonToServer(httpClient, endpoint, body, task);
  }

  /// <summary>
  /// Отправить данные для полного обновления на http-сервер
  /// </summary>
  /// <param name="httpClient">http-клиент</param>
  /// <param name="endpoint">эндпоинт</param>
  /// <param name="body">Тело запроса</param>
  /// <param name="errorAction">Действие при возникновении ошибки</param>
  public static async Task PutJsonToServer(HttpClient httpClient, string endpoint, object? body, Task errorAction)
  {
    ArgumentNullException.ThrowIfNull(httpClient);
    ArgumentNullException.ThrowIfNull(endpoint);

    try
    {
      var httpContent = new StringContent(body == null ? string.Empty : JsonSerializer.Serialize(body));
      httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      var response = await httpClient.PutAsync(endpoint, httpContent);
      response.EnsureSuccessStatusCode();

      if (response.StatusCode is HttpStatusCode.NoContent) return;

      errorAction.Start();
      await errorAction;
    }
    catch (Exception e)
    {
      errorAction.Start();
      await errorAction;
      WriteException(e, endpoint);
    }
  }

  /// <summary>
  /// Удалить данные с сервера
  /// </summary>
  /// <param name="httpClient">http-клиент</param>
  /// <param name="endpoint">Адрес запроса</param>
  /// <param name="errorMessage">Сообщение об ошибке</param>
  public static async Task DeleteFromServer(HttpClient httpClient, string endpoint, string errorMessage)
  {
    var task = new Task(() => Console.Error.WriteLine(errorMessage));
    await DeleteFromServer(httpClient, endpoint, task);
  }

  /// <summary>
  /// Удалить данные с сервера
  /// </summary>
  /// <param name="httpClient">http-клиент</param>
  /// <param name="endpoint">Адрес запроса</param>
  /// <param name="errorAction">Действие при возникновении ошибки</param>
  public static async Task DeleteFromServer(HttpClient httpClient, string endpoint, Task errorAction)
  {
    ArgumentNullException.ThrowIfNull(httpClient);
    ArgumentNullException.ThrowIfNull(endpoint);

    try
    {
      var response = await httpClient.DeleteAsync(endpoint);
      response.EnsureSuccessStatusCode();

      if (response.StatusCode is HttpStatusCode.NoContent) return;

      errorAction.Start();
      await errorAction;
    }
    catch (Exception e)
    {
      errorAction.Start();
      await errorAction;
      WriteException(e, endpoint);
    }
  }

  private static void WriteException(Exception e, string endpoint)
  {
    Console.Error.WriteLine($"Обращение к эндпоинту {endpoint} привело к ошибке -> {e.Message}");
    Console.Error.WriteLine($"{e.StackTrace}");
  }
}
