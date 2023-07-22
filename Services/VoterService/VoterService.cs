#pragma warning disable
using ArzonOL.Models;
using ArzonOL.Repositories.Interfaces;
using ArzonOL.Services.VoterService.Interfaces;
using AutoMapper;
using ArzonOL.Entities;
using ArzonOL.Dtos.ProductVoterDtos;

namespace ArzonOL.Services.VoterService
{
    public class VoterService : IVoterService
    {
        private readonly ILogger<VoterService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public VoterService(ILogger<VoterService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async ValueTask<ProductVoterModel> CreateAsync(ProductVoterModel productVoterModel)
        {
            // Validate input data before creating a new voter
            if (productVoterModel == null)
                throw new ArgumentNullException(nameof(productVoterModel), "Product voter model cannot be null.");

            if (productVoterModel.UserId == Guid.Empty)
                throw new ArgumentException("Invalid user ID.", nameof(productVoterModel.UserId));

            if (productVoterModel.ProductId == Guid.Empty)
                throw new ArgumentException("Invalid product ID.", nameof(productVoterModel.UserId));


            if (productVoterModel.Vote < 0 || productVoterModel.Vote > 5)
                throw new ArgumentException("Invalid vote value. Vote must be between 0 and 5.", nameof(productVoterModel.Vote));

            // Map ProductVoterModel to ProductVoterEntity
            var voterEntity = ModelToEntity(productVoterModel);

            // Your implementation to add a new product voter entity to the repository
            await _unitOfWork.VoterRepository.AddAsync(voterEntity);

            _logger.LogInformation($"Product voter with ID {productVoterModel.UserId} was created.");

            // Map back ProductVoterEntity to ProductVoterModel and return
            return EntityToModel(voterEntity);
        }

        public async ValueTask<ProductVoterModel> UpdateForVoteAsync(ProductVoterModel productVoterModel)
        {
            // Validate input data before updating the vote
            if (productVoterModel == null)
                throw new ArgumentNullException(nameof(productVoterModel), "Product voter model cannot be null.");

            if (productVoterModel.UserId == Guid.Empty)
                throw new ArgumentException("Invalid user ID.", nameof(productVoterModel.UserId));

            if (productVoterModel.ProductId == Guid.Empty)
                throw new ArgumentException("Invalid product ID.", nameof(productVoterModel.UserId));

            if (productVoterModel.Vote < 0 || productVoterModel.Vote > 5)
                throw new ArgumentException("Invalid vote value. Vote must be between 0 and 5.", nameof(productVoterModel.Vote));

            // Map ProductVoterModel to ProductVoterEntity
            var voterEntity = ModelToEntity(productVoterModel);

            // Your implementation to update the vote in the repository
            var existingVoterEntity = _unitOfWork.VoterRepository.GetAll().FirstOrDefault( x => x.UserId == productVoterModel.UserId && x.ProductId == productVoterModel.ProductId);
            if (existingVoterEntity != null)
            {
                existingVoterEntity.Vote = voterEntity.Vote;

                await _unitOfWork.VoterRepository.Update(existingVoterEntity);

                _logger.LogInformation($"Product voter with ID {productVoterModel.UserId} was updated for vote.");
                
                // Map back updated ProductVoterEntity to ProductVoterModel and return
                return EntityToModel(existingVoterEntity);
            }

            throw new KeyNotFoundException("Product voter not found.");
        }

        public async ValueTask<ProductVoterModel> UpdateForCommentAsync(ProductVoterModel productVoterModel)
        {
            // Validate input data before updating the comment
            if (productVoterModel == null)
                throw new ArgumentNullException(nameof(productVoterModel), "Product voter model cannot be null.");

            if (productVoterModel.UserId == Guid.Empty)
                throw new ArgumentException("Invalid user ID.", nameof(productVoterModel.UserId));
            
            if (productVoterModel.ProductId == Guid.Empty)
                throw new ArgumentException("Invalid product ID.", nameof(productVoterModel.UserId));

            // Map ProductVoterModel to ProductVoterEntity
            var voterEntity = ModelToEntity(productVoterModel);

            // Your implementation to update the comment in the repository
            var existingVoterEntity = _unitOfWork.VoterRepository.GetAll().FirstOrDefault( x => x.UserId == productVoterModel.UserId && x.ProductId == productVoterModel.ProductId);
            if (existingVoterEntity != null)
            {
                existingVoterEntity.Comment = voterEntity.Comment;

                await _unitOfWork.VoterRepository.Update(existingVoterEntity);

                _logger.LogInformation($"Product voter with ID {productVoterModel.UserId} was updated for comment.");

                // Map back updated ProductVoterEntity to ProductVoterModel and return
                return EntityToModel(existingVoterEntity);
            }

            throw new KeyNotFoundException("Product voter not found.");
        }

        public async ValueTask<ProductVoterModel> DeleteCommentAsync(ProductVoterModel productVoterModel)
        {
            // Validate input data before deleting the comment
            if (productVoterModel == null)
                throw new ArgumentNullException(nameof(productVoterModel), "Product voter model cannot be null.");

            if (productVoterModel.UserId == Guid.Empty)
                throw new ArgumentException("Invalid user ID.", nameof(productVoterModel.UserId));

            // Map ProductVoterModel to ProductVoterEntity
            var voterEntity = ModelToEntity(productVoterModel);

            // Your implementation to delete the comment in the repository
            var existingVoterEntity = _unitOfWork.VoterRepository.GetAll().FirstOrDefault( x => x.UserId == productVoterModel.UserId && x.ProductId == productVoterModel.ProductId);
            if (existingVoterEntity != null)
            {
                existingVoterEntity.Comment = null;
                await _unitOfWork.VoterRepository.Update(existingVoterEntity);

                _logger.LogInformation($"Comment for product voter with ID {productVoterModel.UserId} was deleted.");

                // Map back updated ProductVoterEntity to ProductVoterModel and return
                return EntityToModel(existingVoterEntity);
            }

            throw new KeyNotFoundException("Product voter not found.");
        }

        public async ValueTask<ProductVoterModel> DeleteVoteAsync(ProductVoterModel productVoterModel)
        {
            // Validate input data before deleting the vote
            if (productVoterModel == null)
                throw new ArgumentNullException(nameof(productVoterModel), "Product voter model cannot be null.");

            if (productVoterModel.UserId == Guid.Empty)
                throw new ArgumentException("Invalid user ID.", nameof(productVoterModel.UserId));

            // Map ProductVoterModel to ProductVoterEntity
            var voterEntity = ModelToEntity(productVoterModel);

            // Your implementation to delete the vote in the repository
            var existingVoterEntity = _unitOfWork.VoterRepository.GetAll().FirstOrDefault( x => x.UserId == productVoterModel.UserId && x.ProductId == productVoterModel.ProductId);
            if (existingVoterEntity != null)
            {
                existingVoterEntity.Vote = null;
                await _unitOfWork.VoterRepository.Update(existingVoterEntity);

                _logger.LogInformation($"Vote for product voter with ID {productVoterModel.UserId} was deleted.");

                // Map back updated ProductVoterEntity to ProductVoterModel and return
                return EntityToModel(existingVoterEntity);
            }

            throw new KeyNotFoundException("Product voter not found.");
        }

        public async ValueTask<ProductVoterModel> MupDtoToModel(VoterDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Product voter model cannot be null.");

            if (dto.UserId == Guid.Empty)
                throw new ArgumentException("Invalid user ID.", nameof(dto.UserId));
            
            if (dto.ProductId == Guid.Empty)
                throw new ArgumentException("Invalid user ID.", nameof(dto.UserId));
            
            var product =  _unitOfWork.ProductRepository.GetAll().FirstOrDefault(x => x.Id == dto.ProductId);
            var voter = _unitOfWork.UserRepository.GetAll().FirstOrDefault(x => x.Id == dto.UserId.ToString());

            return new ProductVoterModel
            {
                Vote = dto.Vote,
                Product = product,
                Comment = dto.Comment,
                User = new PublicUserModel
                            {
                                Id = voter!.Id,
                                Name = voter!.UserName,
                            },
                ProductId = dto.ProductId,
                UserId = dto.UserId
            };
        }

        private  ProductVoterModel EntityToModel(ProductVoterEntity entity)
        {
            var voteResult = GetProductVoteResult(entity.ProductId);
            var boughtCount = GetBoughtCount(entity.ProductId);
            var productPhotos = GetProductMedias(entity.ProductId);
            var productVoters = GetProductVoters(entity.ProductId);
            var user = _unitOfWork.UserRepository.GetAll().FirstOrDefault(x => x.Id == entity.UserId.ToString());
            return new ProductVoterModel
            {
              Id = entity.Id,
              User = new PublicUserModel
                         {
                            Id = user.Id,
                            Name = user.UserName
                         },
              Product = new BaseProductEntity
              {
                        Id = entity.Id,
                        CreatedAt = entity.CreatedAt,
                        Name = entity.Product.Name,
                        OldPrice = entity.Product.OldPrice,
                        NewPrice = entity.Product.NewPrice,
                        VideoUrl = entity.Product.VideoUrl,
                        Description = entity.Product.Description,
                        Brand = entity.Product.Brand,
                        Latitudes = entity.Product.Latitudes,
                        Longitudes = entity.Product.Longitudes,
                        Region = entity.Product.Region,
                        Destrict = entity.Product.Destrict,
                        PhoneNumber = entity.Product.PhoneNumber,
                        StartDate = entity.Product.StartDate,
                        EndDate = entity.Product.EndDate,
                        ProductMedias = productPhotos,
                        Discount = entity.Product.Discount,
              },
              ProductId = entity.ProductId,
              Vote = entity.Vote,
              UserId = entity.UserId,
              Comment = entity.Comment
            };
        }

        private static ProductVoterEntity ModelToEntity(ProductVoterModel model)
        {
            return new ProductVoterEntity
            {
              Id = model.Id,
              User = new UserEntity
              {
                UserName = model.User!.Name,
              },
              Product = model.Product,
              ProductId = model.ProductId,
              Vote = model.Vote,
              UserId = model.UserId,
              Comment = model.Comment
            };
        }
        private List<ProductVoterModel> GetProductVoters(Guid? id)
        => _unitOfWork.VoterRepository.GetAll().Where(x => x.ProductId == id).Select(x => 
        new ProductVoterModel
        {
        Vote = x.Vote,
        Comment = x.Comment,
        User = new PublicUserModel
        {
            Id = _unitOfWork.VoterRepository.GetAll().FirstOrDefault(id => x.Id == x.Id)!.User!.Id,
            Name = _unitOfWork.VoterRepository.GetAll().FirstOrDefault(id => x.Id == x.Id)!.User!.UserName,
        }
        }).ToList();

        private  string GetProductMedias(Guid? id)
        =>  _unitOfWork.ProductRepository.GetAll().Where(x => x.Id == id).Select(media => media.ProductMedias).FirstOrDefault();
        
        private long GetBoughtCount(Guid? id)
        => _unitOfWork.BoughtProductRepository.GetAll().Where(x => x.ProductId == id).Count();

        private float? GetProductVoteResult(Guid? productId)
        {
            var productVoters = _unitOfWork.VoterRepository.GetAll().Where(x => x.ProductId == productId).ToList();

            if(productVoters.Count == 0)
            return 0;
            
            return productVoters.Select(x => x.Vote).Sum()/productVoters.Count();
        }
    }
}
