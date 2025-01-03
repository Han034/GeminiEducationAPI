using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Features.Products.Quaries.GetProductById
{
	public class GetProductByIdQuery : IRequest<GetProductByIdDto>
	{
		public int Id { get; set; }
	}
}
/*
 GetProductByIdQuery: Bu sınıf, belirli bir ürünü Id'sine göre getirmek için kullanılacak sorgu nesnesidir.
IRequest<GetProductByIdDto>: Bu sorgunun GetProductByIdDto tipinde bir sonuç döndüreceğini belirtir.
Id: Getirilmek istenen ürünün Id'si.
 */
