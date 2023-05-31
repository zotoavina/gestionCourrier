﻿using gestion_courrier_bo.Context;
using gestion_courrier_bo.Models;
using Microsoft.EntityFrameworkCore;

namespace gestion_courrier_bo.Services
{
    public class EmployeService : IEmployeService
    {
        private readonly gestion_courrier_bo.Context.AppDbContext _context;

        public EmployeService(AppDbContext context)
        {
            _context = context;
        }

        public Employe findEmployeByEmail(string email)
        {
            return  _context.Employes.FirstOrDefault(u => u.Email == email);
        }

        public List<Employe> findEmployesByRole(string posteCode)
        {
            return _context.Employes.Where(e => e.Poste.code == posteCode).ToList();
        }
    }
}
