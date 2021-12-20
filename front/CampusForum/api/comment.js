import * as config from '../utils/config.js'
export const modelUrl = config.baseUrl +  `/comment`

// 插入评论
export function insert(stateId, fatherId, replyId, text) {
	let url = modelUrl + `/insert`
	let token = config.getToken()
	
	let commentId = 1
	return commentId
}

// 删除评论
export function deleteComment(commentId) {
	let url = modelUrl + `/delete/${commentId}`
	let token = config.getToken()
	
	return true
}

// 根据id获取评论
export function select(commentId) {
	let url = modelUrl + `/select/${commentId}`
	let token = config.getToken()
	
	return {}
}

// 获取一个状态的所有一级评论
export function selectAllByStateId(stateId, page=0, pageSize=10) {
	let url = modelUrl + `/selectAll/${stateId}`
	let token = config.getToken()
	
	return {
		total: 1,
		item: []
	}
}

// 获取一个评论的所有子评论
export function selectAllReply(commentId, page=0, pageSize=10) {
	let url = modelUrl + `/selectAllReply/${stateId}`
	let token = config.getToken()
	
	return {
		total: 1,
		item: []
	}
}