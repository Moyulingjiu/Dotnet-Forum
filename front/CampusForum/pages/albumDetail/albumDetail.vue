<template>
	<view>
		<button type="primary" @click="chooseAndUploadPic()">请选择图片</button>

		<view class="container" :style="{'--picWidth':this.picWidth}">
			<view v-for="(item,index) in this.picture.Item">
				<template>
					<image :src="item.url" mode="aspectFill" class="text" @click="clickPic(index)"></image>
				</template>
			</view>
		</view>

		<uni-popup ref="popup_success" type="message">
			<uni-popup-message type="success" message="删除成功" :duration="3000"></uni-popup-message>
		</uni-popup>
	</view>

</template>


<script>
	import * as config from '../../utils/config.js'
	import * as pictureApi from '../../api/picture.js'
	import * as albumApi from '../../api/album.js'
	export default {
		data() {
			return {
				percent: 0,
				delete: 0,
				albumId: 0,
				albumName: "",
				albumDescription: "",
				windowWidth: 0,
				picWidthNum: 0,
				picWidth: "",
				picRowNum: 0,
				pageInfo: {
					page: 0,
					pageSize: 20
				},
				picture: {
					Totle: 3,
					Item: [{
							"Id": "1",
							"Name": "asd",
							"url": "https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg"
						},
						{
							"Id": "2",
							"Name": "asdsad",
							"url": "https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg"
						},
						{
							"Id": "3",
							"Name": "asdddd",
							"url": "https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg"
						}
					]
				}
			}
		},
		onNavigationBarButtonTap: function(e) {
			if (this.delete == 0) {
				this.delete = 1
				uni.showToast({
					title: '进入删除模式，点击相片删除，再次点击删除按钮退出删除模式',
					icon: "error",
					mask: true,
					duration: 2000
				})
			} else {
				this.delete = 0
				uni.showToast({
					title: '退出删除模式',
					icon: "error",
					mask: true,
					duration: 2000
				})
			}

		},
		onLoad(prop) {
			//获取picture数据，还要传page和pagesize
			let len
			this.albumId = prop.albumId;
			this.picture = pictureApi.getPictureByAlbumId(this.albumId, this.pageInfo).then(data => {
				len = data.data.item.length
				this.picture = {
					totle: 0,
					Item: []
				};
				for (let i = 0; i < len; i++) {
					let temp = {
						"id": data.data.item[i].id,
						"name": data.data.item[i].name,
						"url": data.data.item[i].url
					}
					this.picture.Item.push(temp);
					this.picture.Item[i].url = "/api" + String(this.picture.Item[i].url).replace(/\\/g, "/");
				}
			});



			var that = this;
			uni.getSystemInfo({
				success: function(res) {
					that.windowWidth = res.windowWidth;
					that.picRowNum = Math.ceil(Math.max(3, that.windowWidth / 250));
					that.picWidthNum = (that.windowWidth - 1) / that.picRowNum;
					that.picWidth = that.picWidthNum + "px";
				}
			});
		},

		methods: {
			refresh() {
				if (config.checkToken()) {
					let len
					pictureApi.getPictureByAlbumId(this.albumId, this.pageInfo).then(data => {
						len = data.data.item.length
						this.picture = {
							totle: 0,
							Item: []
						};
						console.log(String(data.data.item[0].url).replace(/\\/g, "/"))
						if (len > 0) {
							albumApi.updateAlbum(this.albumId, String(data.data.item[0].url).replace(/\\/g, "/"))
						}
						for (let i = 0; i < len; i++) {
							let temp = {
								"id": data.data.item[i].id,
								"name": data.data.item[i].name,
								"url": data.data.item[i].url
							}
							this.picture.Item.push(temp);

							this.picture.Item[i].url = "/api" + String(this.picture.Item[i].url).replace(/\\/g,
								"/");
						}

					})
				} else {
					uni.redirectTo({
						url: '../login/login'
					})
				}
			},
			clickPic(index) {
				let that = this
				if (this.delete == 0) {
					window.location.href = String(this.picture.Item[index].url).replace(/\/api/g, "http://localhost:20673")
					uni.navigateTo({
						url: String(this.picture.Item[index].url).replace(/\/api/g, "http://localhost:20673")
					});
				} else {
					uni.showModal({
						title: '确定要删除此相片吗?',
						content: ' ',
						success: function(res) {
							if (res.confirm) {
								pictureApi.deletePicture(that.picture.Item[index].id).then(data => {
									if (typeof data === "undefined") {
										uni.showToast({
											title: '服务器错误',
											icon: "error",
											mask: true,
											duration: 2000
										})
									} else if (data.code == 200) {
										that.$refs.popup_success.open('top')
										that.refresh();
									} else {
										uni.showToast({
											title: '删除失败:' + data.msg,
											icon: "error",
											mask: true,
											duration: 2000
										})
									}
								})
							}
						}
					});
				}
			},
			chooseAndUploadPic() {
				let that = this;
				uni.chooseImage({
					count: 1,
					sizeType: ['copressed'],
					success(res) {
						//因为有一张图片， 输出下标[0]， 直接输出地址
						var imgFiles = res.tempFilePaths[0]
						// 上传图片
						// 做成一个上传对象
						var uper = uni.uploadFile({
							// 需要上传的地址
							url: '/api/picture/insert/' + that.albumId,
							header: {
								'token': config.getToken(),
							},
							// filePath  需要上传的文件
							filePath: imgFiles,
							name: 'file',
							success(res1) {
								// 显示上传信息
								that.refresh()
								// uni.navigateTo({	
								//             url: '/pages/albumDetail/albumDetail?albumId='+String(that.albumId),
								// });
							}

						});
						// uper.onProgressUpdate(function(res){
						//     // 进度条等于 上传到的进度
						//     that.percent = res.progress
						//     console.log('上传进度' + res.progress)
						//     console.log('已经上传的数据长度' + res.totalBytesSent)
						//     console.log('预期需要上传的数据总长度' + res.totalBytesExpectedToSend)
						// })
					}
				})

			},

		}
	}
</script>

<style>
	.container {
		/* 定义flex容器 */
		display: flex;
		/*设置容器内部容器的排列方向*/
		flex-direction: row;
		flex-wrap: wrap;
	}

	.green {
		background-color: green;
	}

	.blue {
		background-color: blue;
	}

	.red {
		background-color: red;
	}

	.black {
		background-color: black;
	}

	.orange {
		background-color: orange;
	}

	.text {
		font-size: 20upx;
		width: var(--picWidth);
		height: var(--picWidth);
	}

	.newstate {
		position: fixed;
		right: 20rpx;
		bottom: 200rpx;
	}

	.circle {
		width: 50px;
		height: 50px;
		background-color: #2979FF;
		border-radius: 50%;
	}

	.plus {
		position: absolute;
		top: 3px;
		right: 14px;
		font-size: 30px;
	}
</style>
