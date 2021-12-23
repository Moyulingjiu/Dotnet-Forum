import * as config from '../utils/config.js'
import * as request from './request.js'
export const modelUrl = `/picture`

// 根据相册id获取其图片列表
export function getPictureByAlbumId(id, page = 0, pageSize = 20) {
	let url = modelUrl + `/selectAll/${id}`;
	let data = {
		page: page,
		pageSize: pageSize
	}
	return request.service({
		url: url,
		data: data,
		method: 'GET'
	})
}

export function deletePicture(pritureId) {
	let url = modelUrl + `/delete/${pritureId}`
	return request.service({
		url: url,
		method: 'DELETE'
	})
}
