using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Ranking : IComparable<Ranking>
{
    public int Id { get; set; }
    public string eMail { get; set; }
    public int Score { get; set; }
    public DateTime Date { get; set; }
    public int Phone { get; set; }
    public int Diamantes { get; set; }
    public int Nivel { get; set; }

    public Ranking(int id, string eMail, int score, DateTime date, int Phone, int diamantes, int nivel)
    {
        this.Id = id;
        this.eMail = eMail;
        this.Score = score;
        this.Date = date;
        this.Phone = Phone;
        this.Diamantes = diamantes;
        this.Nivel = nivel;
    }

    public int CompareTo(Ranking other)
    {
        //return this.Score.CompareTo(other.Score);
        // el que recibe > que el que tiene = -1
        // el que recibe < que el que tiene = 1
        // 0
        if (other.Score > this.Score)
        {
            return 1;
        }
        else if (other.Score < this.Score)
        {
            return -1;
        }
        else if (other.Date > this.Date)
        {
            return -1;
        }
        else if (other.Date < this.Date)
        {
            return 1;
        }
        return 0;



        //return other.Score.CompareTo(this.Score);
    }
}
