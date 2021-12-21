import * as config from '../utils/config.js'
import * as request from './request.js'
export const modelUrl = `/album`

// 根据用户id获取其相册
export function getAlbumByUserId() {
	let url = modelUrl + "/selectAll";
	return request.service({
		url: url,
		method: 'GET',
		header: {
			'token': config.getToken(),
		},
	})
}

// 根据相册id获取其图片列表
export function getPictureByAlbumId(id,{page, pageSize=20}) {
	let url = modelUrl + "selectAll/"+id;
	let data = {
		page: page,
		pageSize:pageSize
	}
	return request.service({
		url: url,
		data:data,
		method: 'GET'
	})
}

export function insertAlbum({
	name,
	description
}) {
	let url = modelUrl + `/insert`
	let data = {
		name: name,
		description: description
	}
	return request.service({
		url: url,
		data: data,
		method: 'POST'
	})
}