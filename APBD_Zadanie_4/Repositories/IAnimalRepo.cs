using APBD_Zadanie_4.Models;
using APBD_Zadanie_4.DTOs;

namespace APBD_Zadanie_4.Repositories
{
    public interface IAnimalRepo
    {
        Task<ICollection<Animal>> GetSync(string orderBy);
        Task AddAnimal(AnimalPost animalPost);
        Task UpdateAnimal(int animalID, AnimalPut animalPut);
        Task DeleteAnimal(int animalId);
        Task<bool> DoesExist(int ID);
    }
}