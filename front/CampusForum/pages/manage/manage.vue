<template>
	<view class="big">
		<view class="trade">
			<view class="texts" :class="curr==0?'active':''" data-index="0" @tap="setCurr">
				注册审核
			</view>
			<view class="texts" :class="curr==1?'active':''" data-index="1" @tap="setCurr">
				用户
			</view>
		</view>
		<swiper class="container" :current="curr" @change="setCurr" style="min-height:100vh;">
			<swiper-item style="height: 100%;">
				<scroll-view>
					<view v-for="(item,index) in checkUser.data">
						<view class="person_container">
							<image class="user_avater" :src="item.avater" @click="enterUser(item.id)"></image>
							<view class="user" @click="enterUser(item.id)">
								<text class="user_name">{{ item.name }}</text>
								<br />
								<text class="user_description">{{ item.description }}</text>
							</view>
							<image class="user_check" src="../../static/allow.png" @click="cilckPass(index)"></image>
							<image class="user_check2" src="../../static/deny.png" @click="cilckDeny(index)"></image>
							<view v-if="item.pass!=0" :class="item.pass==1?'follow':'unfollow'">
								{{ item.pass==1?'已通过':'已拒绝' }}
							</view>
						</view>
					</view>
					<view class="bottom_tips">
						<text>{{ (checkUser.page >= checkUser.total - 1) ? bottomTipsNoMore : bottomTips }}</text>
					</view>
				</scroll-view>
			</swiper-item>
			<swiper-item style="height: 100%;">
				<scroll-view>
					<view v-for="(item,index) in allUser.data">
						<view class="person_container">
							<image class="user_avater" :src="item.avater" @click="enterUser(item.id)"></image>
							<view class="user" @click="enterUser(item.id)">
								<text class="user_name">{{ item.name }}</text>
								<br />
								<text class="user_description">{{ item.description }}</text>
							</view>
							<!-- <view :class="item.disbale?'follow':'unfollow'" @click="clickBan()">
								{{ item.disbale?'封禁':'已封禁' }}
							</view> -->
						</view>
					</view>
					<view class="bottom_tips">
						<text>{{ (allUser.page >= allUser.total - 1) ? bottomTipsNoMore : bottomTips }}</text>
					</view>
				</scroll-view>
			</swiper-item>
		</swiper>
		
		<uni-popup ref="popup_success_allow" type="message">
			<uni-popup-message type="success" message="已通过注册" :duration="1000"></uni-popup-message>
		</uni-popup>
		<uni-popup ref="popup_success_deny" type="message">
			<uni-popup-message type="info" message="已拒绝注册" :duration="1000"></uni-popup-message>
		</uni-popup>
		<uni-popup ref="popup_success_refresh" type="message">
			<uni-popup-message type="success" message="刷新成功" :duration="1000"></uni-popup-message>
		</uni-popup>
	</view>
</template>

<script>
	import * as userApi from "../../api/user.js"
	import * as config from "../../utils/config.js"
	
	export default {
		data() {
			return {
				bottomTips: '- 上拉加载更多 -', // 底部提示
				bottomTipsNoMore: '- 到底了 -', // 底部提示
				curr: 0,
				checkUser: {
					page: 0,
					total: 1,
					data: [
						{
							id: 1,
							name: "facedawn",
							avater: '',
							pass: 0,
							description: '这个人没有个性签名'
						}
					]
				},
				allUser: {
					page: 0,
					total: 1,
					data: [
						{
							id: 1,
							name: "facedawn",
							avater: '',
							pass: 0,
							description: '这个人没有个性签名'
						}
					]
				},
				loadLock: false
			}
		},
		onLoad() {
		},
		onShow() {
			this.refresh()
		},
		onPullDownRefresh() {
			this.refresh()
			uni.stopPullDownRefresh();
			this.$refs.popup_success_refresh.open('top')
		},
		onReachBottom() {
			if (!this.loadLock) { // 加锁
				this.loadLock = true
				if (this.curr == 0) {
					if (this.checkUser.page < this.checkUser.total - 1) {
						this.loadData(this.checkUser.page + 1, this.allUser.page)
					}
				} else {
					if (this.allUser.page < this.allUser.total - 1) {
						this.loadData(this.checkUser.page, this.allUser.page + 1)
					}
				}
				this.loadLock = false
			}
		},
		methods: {
			refresh() {
				if (!config.checkToken()) {
					uni.redirectTo({
						url: '../login/login'
					})
				}
				this.checkUser = {
					page: -1,
					total: 1,
					data: []
				}
				this.allUser = {
					page: -1,
					total: 1,
					data: []
				}
				this.loadData(0,0)
			},
			loadData(checkPage, allPage) {
				if (checkPage != this.checkUser.page) {
					this.checkUser.page = checkPage
					userApi.selectAllRegisterUser().then(data => {
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
							this.checkUser.total = data.data.total
							for (let key in data.data.items) {
								let item = {
									id: data.data.items[key].id,
									name: data.data.items[key].name,
									avater: data.data.items[key].avater,
									description: data.data.items[key].description,
									pass: 0
								}								
								item.avater = "/api" + String(item.avater).replace(/\\/g, "/")
								this.checkUser.data.push(item)
							}
						}
					})
				}
				if (allPage != this.allUser.page) {
					this.allUser.page = allPage
					userApi.selectAll().then(data => {
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
							this.allUser.total = data.data.total
							for (let key in data.data.items) {
								let item = {
									id: data.data.items[key].id,
									name: data.data.items[key].name,
									avater: data.data.items[key].avater,
									description: data.data.items[key].description,
									disbale: false
								}								
								item.avater = "/api" + String(item.avater).replace(/\\/g, "/")
								this.allUser.data.push(item)
							}
						}
					})
				}
			},
			enterUser(userId) {
				uni.navigateTo({
					url: `/pages/otherUsers/otherUsers?id=${userId}`
				})
			},
			setCurr(e) {
				let thisCurr = e.detail.current || e.currentTarget.dataset.index || 0;
				this.curr = thisCurr;
			},
			clickBan(id) {
				console.log(id + "ban")
			},
			cilckPass(index) {
				userApi.passRegister(this.checkUser.data[index].id).then(data => {
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
						this.checkUser.data[index].pass = 1
						this.$refs.popup_success_allow.open('top')
					}
				})
			},
			cilckDeny(index) {
				userApi.rejectRegister(this.checkUser.data[index].id).then(data => {
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
						this.checkUser.data[index].pass = 2
						this.$refs.popup_success_deny.open('top')
					}

				})
			},
			pageChange(e) {
				//重新加载applicationList
				this.applicationList.page = e.current;
				console.log(this.applicationList.page);
			},
			tabtap(index) {
				this.tabIndex = index;
			}
		}
	}
</script>

<style>
	.container {
		/* height: 700rpx; */
	}
	
	.idRow {
		width: 10%;
	}

	.usernameRow {
		width: 49%;
	}

	.passButton {
		background-color: #18BC37;
	}

	.cancelButton {
		background-color: #E43D33;
	}

	.tabs {
		flex: 1;
		flex-direction: column;
		overflow: hidden;
		background-color: #ffffff;
		/* #ifdef MP-ALIPAY || MP-BAIDU */
		height: 100vh;
		/* #endif */
	}

	.scroll-h {
		width: 750upx;
		height: 80upx;
		flex-direction: row;
		/* #ifndef APP-PLUS */
		white-space: nowrap;
		/* #endif */
		/* flex-wrap: nowrap; */
		/* border-color: #cccccc;
	    border-bottom-style: solid;
	    border-bottom-width: 1px; */
	}

	.line-h {
		height: 1upx;
		background-color: #cccccc;
	}

	.uni-tab-item {
		/* #ifndef APP-PLUS */
		display: inline-block;
		/* #endif */
		flex-wrap: nowrap;
		padding-left: 34upx;
		padding-right: 34upx;
	}

	.uni-tab-item-title {
		color: #555;
		font-size: 30upx;
		height: 80upx;
		line-height: 80upx;
		flex-wrap: nowrap;
		/* #ifndef APP-PLUS */
		white-space: nowrap;
		/* #endif */
	}

	.uni-tab-item-title-active {
		color: #007AFF;
	}

	.trade {
		margin-top: 30rpx;
		width: 100%;
		overflow: auto;
	}

	.trade view {
		text-align: center;
		width: 50%;
		float: left;
		font-size: 40rpx;
		font-weight: bold;
		padding-bottom: 10rpx;
	}

	.trade .texts.active {
		border-bottom: 8rpx solid #00A1D6;
	}

	.person_container {
		margin-bottom: 20rpx;
		padding: 10rpx;
		display: flex;
	}

	.user_avater {
		width: 100rpx;
		height: 100rpx;
		border-radius: 50%;
	}

	.user {
		margin-left: 15rpx;
	}

	.user_name {
		font-size: 45rpx;
		font-weight: bold;
	}

	.user_description {
		font-size: 35rpx;
		color: #555555;
	}
	
	.user_check {
		position: absolute;
		right: 100rpx;
		margin-top: 30rpx;
		height: 60rpx;
		width: 60rpx;
	}
	
	.user_check2 {
		position: absolute;
		right: 30rpx;
		margin-top: 30rpx;
		height: 60rpx;
		width: 60rpx;
	}

	.follow {
		position: absolute;
		right: 30rpx;
		font-size: 40rpx;
		color: white;
		margin-top: 30rpx;
		width: 200rpx;
		height: 60rpx;
		text-align: center;
		background: #83cbac;
	}

	.unfollow {
		position: absolute;
		right: 30rpx;
		font-size: 40rpx;
		color: white;
		margin-top: 30rpx;
		width: 200rpx;
		height: 60rpx;
		text-align: center;
		background: #ed5a65;
	}
	
	.bottom_tips {
		margin-top: 30rpx;
		text-align: center;
		height: 200rpx;
	}

	.bottom_tips text {
		color: #A8A8A8;
		bottom: 0;
	}
</style>
