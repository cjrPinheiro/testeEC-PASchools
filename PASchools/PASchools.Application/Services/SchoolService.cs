using AutoMapper;
using PASchools.Application.DTOs;
using PASchools.Application.Interfaces;
using PASchools.Domain.Entities;
using PASchools.Google.Connector;
using PASchools.Google.Connector.Interfaces;
using PASchools.Google.Connector.Models;
using PASchools.Google.Connector.Models.Requests;
using PASchools.Google.Connector.Models.Response;
using PASchools.Persistence.Interfaces;
using PASchools.SIE.Connector;
using PASchools.SIE.Connector.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
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
        public async Task<List<SchoolDTO>> FindSchoolsByAddressOrderByDistance(Coordinate origin)
        {
            try
            {
                List<School> schools = (await _schoolRepository.GetAllSchoolsAsync()).ToList();
                var schoolsDTO = _mapper.Map<List<SchoolDTO>>(schools).Take(10).ToArray();

                var destCoordinates = schoolsDTO.Select(q => new Coordinate() { Lat = q.Address.Latitude, Lng = q.Address.Longitude }).ToArray();
                DistanceResponse response = null;
                if (destCoordinates != null && destCoordinates.Length > 25)
                {
                    var iterations = (destCoordinates.Length / 25);
                    iterations += destCoordinates.Length % 25 > 0 ? 1 : 0;
                    for (int i = 0; i < iterations; i++)
                    {
                        var auxDestCoordinates = destCoordinates.Skip(i * 25).Take(25).ToArray();

                        if (response != null)
                        {
                            var auxResponse = await _googleApiClient.GetDistanceBetweenOneOriginManyDestinationAsync(origin, auxDestCoordinates);
                            if (auxResponse != null && auxResponse.status == "OK")
                            {
                                response.rows.First().elements.AddRange(auxResponse.rows.First().elements);
                            }
                        }
                        else
                        {
                            response = await _googleApiClient.GetDistanceBetweenOneOriginManyDestinationAsync(origin, auxDestCoordinates);
                            if (response != null && response.status != "OK")
                            {
                                response = null;
                            }
                        }

                    }
                }
                else
                {
                    response = await _googleApiClient.GetDistanceBetweenOneOriginManyDestinationAsync(origin, destCoordinates);
                }
                if (response != null && response.status == "OK")
                {
                    if (response.rows.Any() && response.rows.First().elements.Any())
                    {
                        var elements = response.rows.First().elements;

                        for (int i = 0; i < elements.Count; i++)
                        {
                            if (elements[i].distance != null)
                            {
                                schoolsDTO[i].Distance = elements[i].distance.value;
                                schoolsDTO[i].DistanceText = elements[i].distance.text;
                            }

                        }
                    }
                }
                return schoolsDTO.OrderBy(q=> q.Distance).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RouteDTO> GenerateRoute(Coordinate origin, Coordinate destination)
        {
            RouteDTO rsp = null;
            try
            {
                var response = await _googleApiClient.GetRouteBetweenTwoCoordinatesAsync(origin, destination);

                if (response != null && response.status == "OK")
                {
                    if (response.routes.Any() && response.routes.First().legs.Any())
                    {
                        var leg = response.routes.First().legs.First();

                        rsp = new();
                        rsp.DurationText = leg.duration != null ? $"Distância total: {leg.distance.text}" : "Não foi possível calcular a distância total da rota";
                        rsp.DistanceText = leg.distance != null ? $"Duração aproximada: {leg.duration.text}" : "Não foi possível calcular a duração total da rota";

                        if (leg.steps.Any())
                        {
                            rsp.Steps = new();
                            foreach (var step in leg.steps)
                            {
                                rsp.Steps.Add($"Trajeto: {step.distance.text} - Instrução: <br/> {step.html_instructions}");
                            }
                        }
                    }
                }
                return rsp;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Coordinate> GetCoordinatesAsync(AddressDTO origin)
        {
            Coordinate rsp = null;
            try
            {
                var response = await _googleApiClient.GetGeocodingAsync(origin);

                if (response != null && response.status == "OK")
                {
                    if (response.results.Any())
                    {
                        if (response.results.First().geometry != null && response.results.First().geometry.location != null)
                        {
                            rsp = new Coordinate()
                            {
                                Lat = response.results.First().geometry.location.lat,
                                Lng = response.results.First().geometry.location.lng

                            };
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return rsp;
        }

        public async Task<int> UpdateSchoolDatabase(int limit)
        {
            int countReg = 0;
            try
            {
                var response = await _sieApiClient.GetAllPASchoolsListAsync(limit);

                foreach (var item in response.result.records)
                {
                    School school = await _schoolRepository.GetSchoolsByInepIdAsync(item.codigo);
                    if (school == null)
                    {
                        try
                        {
                            school = new School()
                            {
                                ActiveOrganization = item.situacao_funcionamento?.Trim() == "EM ATIVIDADE",
                                Code = (short)item.codigo,
                                EducationType = item.tipo?.Trim(),
                                Email = item.email?.Trim(),
                                InepCode = item.inep,
                                Name = item.nome?.Trim(),
                                PhoneNumber = item.telefone?.Trim(),
                                PublicDepartment = item.dep_administrativa?.Trim() == "MUNICIPAL",
                                WebSite = item.url_website?.Trim(),
                                Address = new Address()
                                {
                                    City = "Porto Alegre",
                                    District = item.bairro?.Trim(),
                                    Latitude = item.latitude,
                                    Longitude = item.longitude,
                                    Street = item.logradouro?.Trim(),
                                    Number = item.numero,
                                    PostalCode = item.cep,
                                    State = "SC"
                                }
                            };

                            if (school.Address.Latitude == 0 && school.Address.Longitude == 0)
                            {
                                var address = _mapper.Map<AddressDTO>(school.Address);
                                var coord = await GetCoordinatesAsync(address);

                                school.Address.Latitude = coord.Lat;
                                school.Address.Longitude = coord.Lng;
                            }

                            await _schoolRepository.AddAsync(school);
                            countReg++;
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

                            school.ActiveOrganization = item.situacao_funcionamento?.Trim() == "EM ATIVIDADE";
                            school.Code = (short)item.codigo;
                            school.EducationType = item.tipo?.Trim();
                            school.Email = item.email?.Trim();
                            school.InepCode = item.inep;
                            school.Name = item.nome?.Trim();
                            school.PhoneNumber = item.telefone?.Trim();
                            school.PublicDepartment = item.dep_administrativa == "MUNICIPAL";
                            school.WebSite = item.url_website?.Trim();
                            if (school.Address == null)
                            {
                                school.Address = new Address()
                                {
                                    City = "Porto Alegre",
                                    District = item.bairro?.Trim(),
                                    Latitude = item.latitude,
                                    Longitude = item.longitude,
                                    Street = item.logradouro?.Trim(),
                                    Number = item.numero,
                                    PostalCode = item.cep,
                                    State = "SC"
                                };
                            }
                            else
                            {
                                school.Address.City = "Porto Alegre";
                                school.Address.District = item.bairro?.Trim();
                                school.Address.Latitude = item.latitude;
                                school.Address.Longitude = item.longitude;
                                school.Address.Street = item.logradouro?.Trim();
                                school.Address.Number = item.numero;
                                school.Address.PostalCode = item.cep;
                                school.Address.State = "SC";
                            }
                            if (school.Address.Latitude == 0 && school.Address.Longitude == 0)
                            {
                                var address = _mapper.Map<AddressDTO>(school.Address);
                                var coord = await GetCoordinatesAsync(address);

                                school.Address.Latitude = coord.Lat;
                                school.Address.Longitude = coord.Lng;
                            }

                            _schoolRepository.Update(school);
                            countReg++;
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

            return countReg;
        }
    }
}
