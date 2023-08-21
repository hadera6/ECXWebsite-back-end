using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Subscription_.Request.Command;
using ECX.Website.Application.DTOs.Subscription;
using ECX.Website.Application.DTOs.Subscription.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Subscription_.Handler.Command
{
    public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionCommand, BaseCommonResponse>
    {
        private ISubscriptionRepository _subscriptionRepository;
        private IMapper _mapper;
        public UpdateSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new SubscriptionUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.SubscriptionFormDto);
            var SubscriptionDto = _mapper.Map<SubscriptionDto>(request.SubscriptionFormDto);
            var flag = await _subscriptionRepository.Exists(request.SubscriptionFormDto.Id);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Update Failed";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                response.Status = "400";
            }
            else if (flag == false)
            {

                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Subscription), request.SubscriptionFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.SubscriptionFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.SubscriptionFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _subscriptionRepository.GetById(
                                request.SubscriptionFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.SubscriptionFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.SubscriptionFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            SubscriptionDto.ImgName = fileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Success = false;
                        response.Message = "Update Failed";
                        response.Errors = new List<string> { ex.Message };
                        response.Status = "400";
                    }
                }
                else
                {
                    SubscriptionDto.ImgName = (await _subscriptionRepository.GetById(
                                request.SubscriptionFormDto.Id)).ImgName;
                } 

                var updateData = await _subscriptionRepository.GetById(request.SubscriptionFormDto.Id);
                
                _mapper.Map(SubscriptionDto, updateData);

                var data = await _subscriptionRepository.Update(updateData);

                response.Data = _mapper.Map<SubscriptionDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

