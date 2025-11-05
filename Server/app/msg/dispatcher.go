package msg

import (
	"server/app/work"
	"server/pb/pb"

	"github.com/murang/potato/log"
	"github.com/murang/potato/util"
	"google.golang.org/protobuf/proto"
)

type HandlerFunc[T proto.Message] func(agent *work.Agent, msg T)

func wrapHandler[T proto.Message](handler HandlerFunc[T]) func(agent *work.Agent, msg proto.Message) {
	return func(agent *work.Agent, msg proto.Message) {
		req, ok := msg.(T)
		if !ok {
			log.Sugar.Errorf("msg type not match: %T", msg)
			return
		}
		handler(agent, req)
	}
}

// 消息分发
var msgDispatcher = map[uintptr]func(agent *work.Agent, msg proto.Message){
	util.TypePtrOf(&pb.C2S_Heartbeat{}):      wrapHandler(work.Heartbeat),
	util.TypePtrOf(&pb.C2S_Login{}):          wrapHandler(work.Login),
	util.TypePtrOf(&pb.C2S_GetLevelDetail{}): wrapHandler(work.GetLevelDetail),
	util.TypePtrOf(&pb.C2S_FinishLevel{}):    wrapHandler(work.FinishLevel),
}
