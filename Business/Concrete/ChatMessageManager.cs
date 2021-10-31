using Business.Abstract;
using Business.Constants;
using Core.Aspects.Autofac.ErrorHandling;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class ChatMessageManager:IChatMessageService
    {
        IChatMessageDal _chatMessageDal;
        public ChatMessageManager(IChatMessageDal chatMessageDal)
        {
            _chatMessageDal = chatMessageDal;
        }

        [ErrorHandleAspect]
        public IResult Add(ChatMessage chatMessage)
        {
            _chatMessageDal.Add(chatMessage);
            return new SuccessResult(Messages.Ok);
        }

        public IResult GetByDate(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public IResult GetByDate(DateTime date)
        {
            var data = _chatMessageDal.GetAll(m => m.DateTime == date);
            return new SuccessDataResult<List<ChatMessage>>(data,Messages.Ok);
        }

        [ErrorHandleAspect]
        public IResult GetTodaysMessages()
        {
            var data = _chatMessageDal.GetAll(p=>p.DateTime.Date == DateTime.Now.Date);
            return new SuccessDataResult<List<ChatMessage>>(data,Messages.Ok);
        }
    }
}
