using RetailSales.Models;
using RetailSales.Models.Master;
using System.Data;

namespace RetailSales.Interface
{
    public interface ISequenceService
    {
        string SequenceCRUD(Sequence cy);
        DataTable GetAllSequenceGRID(string strStatus);
        DataTable GetEditSequence(string id);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);

    }
}