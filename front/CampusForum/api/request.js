import * as config from '../utils/config.js'

function request({
	url,
	data,
	method
}) {
	return new Promise((resolve, reject) => {
		//uni.app 发起网络请求
		uni.request({
			url: config.baseUrl + url,
			data,
			method,
			sslVerify: true, // 进行ssl验证
			success: ({
				data,
				statusCode,
				header
			}) => {
				if (data.success) {
					//请求成功
					resolve(data)
				} else {
					//请求失败 提示用户
					uni.showToast({
						title: data.message,
						icon: "success",
						mask: true,
						duration: 2000
					});
					//进行失败回调
					reject(data.message)
				}
			},
			fail: (errot) => {
				//请求错误 直接进行错误回调
				reject(errot)
			}
		})
	})
}

export default request