<template>
	<view class="laugh">
		<view class="album_box" v-for="(item,index) in albumArr">
			<image class="cover" :src="item.cover" mode="aspectFill" @click="clickAlbum(item.id)"></image>
			<view class="album_information" @click="clickAlbum(item.id)">
				<text class="album_name">{{ item.name }}</text>
				<br />
				<text class="album_description">{{ item.description }}</text>
			</view>
			<view class="album_button">
				<view class="album_button_edit" @click="editAlbum(item.id)">修改</view>
				<view class="album_button_delete" @click="deleteAlbum(item.id)">删除</view>
			</view>
		</view>

		<view class="newstate" @tap="clickToAddAlbum()">
			<view class="circle"></view>
			<view class="plus">+</view>
		</view>

		<uni-popup ref="popup_success" type="message">
			<uni-popup-message type="success" message="删除成功" :duration="1000"></uni-popup-message>
		</uni-popup>
		<uni-popup ref="popup_success_refresh" type="message">
			<uni-popup-message type="success" message="刷新成功" :duration="1000"></uni-popup-message>
		</uni-popup>
	</view>
</template>

<script>
	import uniList from "@/uni_modules/uni-list/components/uni-list/uni-list.vue" // uni-app列表组件，渲染图片列表
	import uniListItem from "@/uni_modules/uni-list/components/uni-list-item/uni-list-item.vue" // 列表项组件
	import * as config from "../../utils/config.js"
	import * as albumApi from "../../api/album.js"
	export default {
		data() {
			return {
				delete: 0,
				albumArr: [{
					"id": "1",
					"name": "1",
					"description": "testaaaaaaaaaaaaaaaaaaaaaaaa",
					"cover": "https://ns-strategy.cdn.bcebos.com/ns-strategy/upload/fc_big_pic/part-00563-2872.jpg"
				}, ],
			}
		},
		onLoad() {},
		onShow() {
			this.refresh()
		},
		onPullDownRefresh() {
			this.refresh()
			uni.stopPullDownRefresh();
			this.$refs.popup_success_refresh.open('top')
		},
		methods: {
			refresh() {
				this.albumArr = []
				if (config.checkToken()) {
					albumApi.getAlbumByUserId().then(data => {
						if (typeof data === "undefined") {
							uni.showToast({
								title: '服务器错误',
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else if (data.code != 200) {
							uni.showToast({
								title: data.msg,
								icon: "error",
								mask: true,
								duration: 2000
							})
						} else {
							for (let key in data.data.item) {
								let albumItem = {
									id: data.data.item[key].id,
									name: data.data.item[key].name,
									description: data.data.item[key].description,
									cover: data.data.item[key].cover
								}
								albumItem.cover = "/api" + String(albumItem.cover).replace(/\\/g, "/")
								this.albumArr.push(albumItem)
							}
						}
					})
				} else {
					uni.redirectTo({
						url: '../login/login'
					})
				}
			},
			deleteAlbum(albumId) {
				new Promise((resolve, reject) => {
					uni.showModal({
						title: '确定删除此相册吗?',
						content: ' ',
						success: function(res) {
							resolve(res)
						}
					})
				}).then(data => {
					if (data.confirm) {
						return albumApi.deleteAlbum(albumId)
					}
				}).then(data => {
					if (typeof data === "undefined") {
						
					} else if (data.code == 200) {
						this.$refs.popup_success.open('top')
						this.refresh()
					} else {
						uni.showToast({
							title: data.msg,
							icon: "error",
							mask: true,
							duration: 2000
						})
					}
				})
			},
			editAlbum(albumId) {
				uni.navigateTo({
					url: `../albumAdd/albumEdit?id=${albumId}`
				})
			},
			clickAlbum(albumId) {
				uni.navigateTo({
					url: `/pages/albumDetail/albumDetail?id=${albumId}`
				})
			},
			clickToAddAlbum() {
				uni.navigateTo({
					url: '/pages/albumAdd/albumAdd',
				});
			}
		}
	}
</script>

<style>
	.album_box {
		padding: 10rpx;
		margin-top: 10rpx;
		margin-bottom: 10rpx;
		width: 90%;
		margin-left: 5%;
		display: flex;
		height: 200rpx;
		border-bottom: 3rpx solid #F0F0F0;
	}

	.cover {
		width: 200rpx;
		height: 200rpx;
	}

	.album_information {
		margin-top: 30rpx;
		margin-left: 30rpx;
	}

	.album_name {
		font-size: 50rpx;
		font-weight: bold;
	}

	.album_description {
		font-size: 35rpx;
		color: #808080;
	}
	
	.album_button {
		margin-top: 160rpx;
		right: 40rpx;
		position: absolute;
		display: flex;
	}
	
	.album_button_edit {
		font-size: 30rpx;
		color: #93b5cf;
	}
	
	.album_button_edit:hover {
		background-color: #E0E0E0;
		border-radius: 10rpx;
	}
	
	.album_button_delete {
		font-size: 30rpx;
		margin-left: 20rpx;
		color: #ed556a;
	}
	
	.album_button_delete:hover {
		background-color: #E0E0E0;
		border-radius: 10rpx;
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
