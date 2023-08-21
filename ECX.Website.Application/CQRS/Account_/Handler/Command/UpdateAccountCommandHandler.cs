using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Account_.Request.Command;
using ECX.Website.Application.DTOs.Account;
using ECX.Website.Application.DTOs.Account.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Account_.Handler.Command
{
    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, BaseCommonResponse>
    {
        private IAccountRepository _accountRepository;
        private IMapper _mapper;
        public UpdateAccountCommandHandler(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new AccountUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.AccountFormDto);
            var AccountDto = _mapper.Map<AccountDto>(request.AccountFormDto);
            var flag = await _accountRepository.Exists(request.AccountFormDto.Id);

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
                            nameof(Account), request.AccountFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.AccountFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.AccountFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _accountRepository.GetById(
                                request.AccountFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.AccountFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.AccountFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            AccountDto.ImgName = fileName;
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
                    AccountDto.ImgName = (await _accountRepository.GetById(
                                request.AccountFormDto.Id)).ImgName;
                } 

                var updateData = await _accountRepository.GetById(request.AccountFormDto.Id);
                
                _mapper.Map(AccountDto, updateData);

                var data = await _accountRepository.Update(updateData);

                response.Data = _mapper.Map<AccountDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

