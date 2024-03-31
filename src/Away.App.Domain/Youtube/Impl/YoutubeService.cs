﻿using Away.App.Domain.Youtube.Entities;

namespace Away.App.Domain.Youtube.Impl;

[DI(ServiceLifetime.Singleton)]
public class YoutubeService(IYoutubeRepository youtubeRepository) : IYoutubeService
{
    public List<YoutubeEntity> GetList()
    {
        return youtubeRepository.GetList();
    }

    public bool Save(YoutubeEntity entity)
    {
        return youtubeRepository.InsertOrUpdate(entity);
    }

    public bool Remove(YoutubeEntity entity)
    {
        var path = GetFolderPath(entity);

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        return youtubeRepository.DeleteById(entity.Id);
    }

    public string GetFolderPath(YoutubeEntity entity)
    {
        var videoId = YoutubeClient.ParseVideoId(entity.Source);
        return Path.Combine(entity.ImagePath.Split(videoId)[0], videoId);
    }
}
