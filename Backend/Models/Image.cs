public class Image
{
    public int Id {get; set;}
    public string ImgUrl {get; set;}

}
public class ImageDto
{
    public IFormFile? ImageFile {get; set;}
}