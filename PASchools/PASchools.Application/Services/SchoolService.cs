using AutoMapper;
using PASchools.Application.DTOs;
using PASchools.Application.Interfaces;
using PASchools.Domain.Entities;
using PASchools.Google.Connector;
using PASchools.Google.Connector.Interfaces;
using PASchools.Google.Connector.Models;
using PASchools.Google.Connector.Models.Requests;
using PASchools.Persistence.Interfaces;
using PASchools.SIE.Connector;
using PASchools.SIE.Connector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PASchools.Application.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolPersist _schoolRepository;
        private readonly IGoogleApiClient _googleApiClient;
        private readonly ISIEApiClient _sieApiClient;

        private readonly IMapper _mapper;

        public SchoolService(IMapper mapper, ISchoolPersist schoolRepository, IGoogleApiClient googleApiClient, ISIEApiClient sieApiClient)
        {
            _googleApiClient = googleApiClient;
            _schoolRepository = schoolRepository;
            _sieApiClient = sieApiClient;
            _mapper = mapper;
        }
        public async Task<List<SchoolDTO>> FindSchoolsByAddressOrderByDistance(AddressDTO address)
        {
            try
            {
                List<School> schools = (await _schoolRepository.GetAllSchoolsAsync()).ToList();
                var schoolsDTO = _mapper.Map<List<SchoolDTO>>(schools);


                foreach (var item in schoolsDTO)
                {
                    var coordinate = new Coordinate() { Latitude = item.Address.Latitude.ToString(), Longitude = item.Address.Longitude.ToString() };

                    var response = await _googleApiClient.GetDistanceBetweenTwoCoordinatesAsync(address, coordinate);

                    if (response != null && response.status == "OK")
                    {
                        if (response.rows.Any())
                        {
                            if (response.rows.First().elements.Any())
                            {
                                item.Distance = response.rows.First().elements.First().distance.value;
                                item.DistanceText = response.rows.First().elements.First().distance.text;
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<RouteDTO> GenerateRoute(AddressDTO origin, Coordinate destination)
        {
            try
            {
                var response = await _googleApiClient.GetRouteBetweenTwoCoordinatesAsync(origin, destination);

                if (response != null)
                {

                    return new RouteDTO();

                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task UpdateSchoolDatabase()
        {
            try
            {
                var response = await _sieApiClient.GetAllPASchoolsListAsync();

                foreach (var item in response.result.records)
                {
                    School school = await _schoolRepository.GetSchoolsByInepIdAsync(item.codigo);
                    if (school == null)
                    {
                        try
                        {
                            school = new School()
                            {
                                ActiveOrganization = item.situacao_funcionamento == "EM ATIVIDADE",
                                Code = (short)item.codigo,
                                EducationType = item.tipo,
                                Email = item.email,
                                InepCode = item.inep,
                                Name = item.nome,
                                PhoneNumber = item.telefone,
                                PublicDepartment = item.dep_administrativa == "MUNICIPAL",
                                WebSite = item.url_website,
                                Address = new Address()
                                {
                                    City = "Porto Alegre",
                                    District = item.bairro,
                                    Latitude = item.latitude,
                                    Longitude = item.longitude,
                                    Street = item.logradouro,
                                    Number = item.numero,
                                    PostalCode = item.cep,
                                    State = "SC"
                                }
                            };
                            await _schoolRepository.AddAsync(school);
                        }
                        catch (Exception)
                        {
                            // LOG
                        }
                    }
                    else
                    {
                        try
                        {

                            school.ActiveOrganization = item.situacao_funcionamento == "EM ATIVIDADE";
                            school.Code = (short)item.codigo;
                            school.EducationType = item.tipo;
                            school.Email = item.email;
                            school.InepCode = item.inep;
                            school.Name = item.nome;
                            school.PhoneNumber = item.telefone;
                            school.PublicDepartment = item.dep_administrativa == "MUNICIPAL";
                            school.WebSite = item.url_website;
                            if (school.Address == null)
                            {
                                school.Address = new Address()
                                {
                                    City = "Porto Alegre",
                                    District = item.bairro,
                                    Latitude = item.latitude,
                                    Longitude = item.longitude,
                                    Street = item.logradouro,
                                    Number = item.numero,
                                    PostalCode = item.cep,
                                    State = "SC"
                                };
                            }
                            else
                            {
                                school.Address.City = "Porto Alegre";
                                school.Address.District = item.bairro;
                                school.Address.Latitude = item.latitude;
                                school.Address.Longitude = item.longitude;
                                school.Address.Street = item.logradouro;
                                school.Address.Number = item.numero;
                                school.Address.PostalCode = item.cep;
                                school.Address.State = "SC";
                            }

                            _schoolRepository.Update(school);
                        }
                        catch (Exception)
                        {
                            //LOG
                        }
                    }


                }
                await _schoolRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
