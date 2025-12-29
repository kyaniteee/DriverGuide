using System.Linq.Expressions;

namespace DriverGuide.Domain.Interfaces;

/// <summary>
/// Bazowy interfejs repozytorium definiujący standardowe operacje CRUD dla wszystkich encji.
/// Implementuje wzorzec Repository Pattern zapewniający abstrakcję nad warstwą dostępu do danych.
/// </summary>
/// <typeparam name="T">
/// Typ encji domenowej, dla której repozytorium będzie wykonywać operacje.
/// Musi być klasą (class constraint).
/// </typeparam>
public interface IRepositoryBase<T> where T : class
{
    /// <summary>
    /// Pobiera wszystkie rekordy danej encji z bazy danych.
    /// </summary>
    /// <returns>
    /// Kolekcja wszystkich encji typu T.
    /// Zwraca pustą kolekcję jeśli nie znaleziono żadnych rekordów.
    /// </returns>
    Task<ICollection<T>> GetAllAsync();

    /// <summary>
    /// Pobiera encję po jej unikalnym identyfikatorze (klucz główny).
    /// </summary>
    /// <param name="id">Identyfikator encji do pobrania.</param>
    /// <returns>
    /// Encja typu T jeśli została znaleziona, null w przeciwnym przypadku.
    /// </returns>
    Task<T> GetByIdAsync(int id);

    /// <summary>
    /// Pobiera encję po jej unikalnym identyfikatorze w formacie GUID.
    /// </summary>
    /// <param name="guid">Identyfikator GUID encji do pobrania.</param>
    /// <returns>
    /// Encja typu T jeśli została znaleziona, null w przeciwnym przypadku.
    /// </returns>
    Task<T> GetByGuidAsync(Guid guid);

    /// <summary>
    /// Pobiera pierwszą encję spełniającą podane wyrażenie filtrujące.
    /// </summary>
    /// <param name="filter">
    /// Wyrażenie lambda definiujące warunki filtrowania (np. x => x.Name == "test").
    /// </param>
    /// <param name="useNoTracking">
    /// Określa czy użyć AsNoTracking() dla lepszej wydajności przy operacjach tylko do odczytu.
    /// Domyślnie false.
    /// </param>
    /// <returns>
    /// Pierwsza encja spełniająca warunki lub null jeśli nie znaleziono.
    /// </returns>
    Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);

    /// <summary>
    /// Pobiera wszystkie encje spełniające podane wyrażenie filtrujące.
    /// </summary>
    /// <param name="filter">
    /// Wyrażenie lambda definiujące warunki filtrowania.
    /// </param>
    /// <param name="useNoTracking">
    /// Określa czy użyć AsNoTracking() dla lepszej wydajności.
    /// Domyślnie false.
    /// </param>
    /// <returns>
    /// Kolekcja encji spełniających warunki. Zwraca pustą kolekcję jeśli nie znaleziono.
    /// </returns>
    Task<ICollection<T>> FindAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);

    /// <summary>
    /// Tworzy nową encję w bazie danych.
    /// </summary>
    /// <param name="entity">Encja do utworzenia.</param>
    /// <returns>
    /// Utworzona encja z wypełnionym kluczem głównym i innymi wartościami wygenerowanymi przez bazę.
    /// </returns>
    Task<T> CreateAsync(T entity);

    /// <summary>
    /// Aktualizuje istniejącą encję w bazie danych.
    /// </summary>
    /// <param name="entity">
    /// Encja z zaktualizowanymi wartościami.
    /// Musi zawierać poprawny klucz główny.
    /// </param>
    /// <returns>Zaktualizowana encja.</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Usuwa encję z bazy danych.
    /// </summary>
    /// <param name="entity">Encja do usunięcia.</param>
    /// <returns>True jeśli usunięcie powiodło się, false w przeciwnym przypadku.</returns>
    Task<bool> DeleteAsync(T entity);
}
