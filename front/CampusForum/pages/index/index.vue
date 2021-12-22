<template>
	<view>
		<view class="search_box">
			<image class="search_icon" src="../../static/search.png"></image>
			<input class="search_area" type="text" v-model="searchCondition" confirm-type="search" @confirm="search()"
				placeholder="搜索用户状态等" />
			<image class="search_cancel" v-if="searchCondition.length!=0" src="../../static/cancel.png"
				@click="clearCondition()"></image>
		</view>

		<view class="banner_box">
			<swiper class="swiper" indicator-dots="true" autoplay="true" interval="5000" duration="1500">
				<swiper-item v-for="(item , index) in banner" :key="index">
					<image :src="item.img" mode="aspectFill"></image>
				</swiper-item>
			</swiper>
		</view>

		<view class="state_box" v-for="(item,index) in stateList">
			<view class="state_title" @click="stateDetail(index)">
				<text>{{ item.title }}</text>
			</view>
			<view class="state_author" @click="userdetail(index)">
				<image :src="item.userAvater"></image>
				<text>{{ item.userName }}</text>
			</view>
			<view class="state_text" @click="stateDetail(index)">
				<text space="emsp">{{ item.text }}</text>
			</view>
			<view class="state_bottom">
				<text class="state_date">{{ item.gmtCreate }}</text>
				<view class="state_icon">
					<image src="../../static/comment.png" mode="aspectFill" @click="stateDetail(index)"></image>
					<image :src="item.like?likeIcon:unlikeIcon" @click="like(index)"></image>
				</view>
			</view>
		</view>

		<view class="bottom_tips">
			<text>{{ (page >= total - 1) ? bottomTipsNoMore : bottomTips }}</text>
		</view>
		
		<uni-popup ref="popup_success_refresh" type="message">
			<uni-popup-message type="success" message="刷新成功" :duration="2000"></uni-popup-message>
		</uni-popup>
	</view>
</template>

<script>
	import * as stateApi from "../../api/state.js"
	import * as config from "../../utils/config.js"

	export default {
		data() {
			return {
				likeIcon: '../../static/like_active.png', // 点赞图标
				unlikeIcon: '../../static/like.png', // 未点赞图标
				searchCondition: '', // 搜索条件
				bottomTips: '- 上拉加载更多 -', // 底部提示
				bottomTipsNoMore: '- 到底了 -', // 底部提示
				banner: [{
					img: '../../static/banner1.jpg',
					comment: '测试备注消息1'
				}, {
					img: '../../static/banner2.jpg',
					comment: '测试备注消息2'
				}], // 主页轮播图
				bannerCurrent: 0,
				bannerMode: 'round',
				page: 0, // 当前页码
				total: 1, // 最后一页
				stateList: [{
					id: 1,
					title: '测试状态标题1',
					userId: 1, // 用户id
					userName: '墨羽翎玖', // 用户名
					userAvater: '../../static/avater.jpg', // 头像
					text: '测试的正文1',
					share: true,
					gmtCreate: '2021年7月20日 20:00',
					like: true
				}],
				isRefresh: true
			}
		},
		onLoad() {
			// load之后一定会show，所以刷新一定会执行
			this.isRefresh = true
		},
		onShow() {
			this.refresh()
		},
		onPullDownRefresh() {
			this.isRefresh = true
			this.refresh()
			uni.stopPullDownRefresh();
			this.$refs.popup_success_refresh.open('top')
		},
		methods: {
			refresh() {
				if (config.checkToken()) {
					if (this.isRefresh) {
						this.isRefresh = false
						this.stateList = []
						this.page = 0
						this.loadData()
					}
				} else {
					uni.redirectTo({
						url: '../login/login'
					})
				}
			},
			loadData(page = this.page) {
				stateApi.selectAll(page).then(data => {
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
						this.total = data.data.total
						for (let key in data.data.items) {
							if (key != 'length') {
								let stateItem = {
									id: data.data.items[key].id,
									title: data.data.items[key].title,
									userId: data.data.items[key].userId,
									userName: data.data.items[key].userName,
									userAvater: data.data.items[key].userAvater, // 头像
									text: data.data.items[key].text,
									share: data.data.items[key].share_state,
									gmtCreate: data.data.items[key].gmt_create,
									like: data.data.items[key].like
								}
								this.stateList.push(stateItem)
							}
						}
					}
				})
			},
			search() {
				// 搜索
				uni.navigateTo({
					url: `/pages/search/search?searchCondition=`+this.searchCondition
				});
			},
			clearCondition() {
				this.searchCondition = ''
			},
			like(index) {
				let id = this.stateList[index].id
				if (this.stateList[index].like) {
					stateApi.unlike(id).then(data => {
						if (typeof data == "undefined") {
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
							this.stateList[index].like = false
							this.$forceUpdate()
						}
					})
				} else {
					stateApi.like(id).then(data => {
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
							this.stateList[index].like = true
							this.$forceUpdate()
						}
					})
				}
			},
			userdetail(index) {
				let id = this.stateList[index].userId
				uni.navigateTo({
					url: `/pages/otherUsers/otherUsers?id=${id}`
				})
			},
			stateDetail(index) {
				let id = this.stateList[index].id
				uni.navigateTo({
					url: `/pages/stateDetail/stateDetail?id=${id}`
				})
			}
		}
	}
</script>

<style>
	.search_box {
		display: flex;
		background-color: #F3F3F3;
		margin-left: 5%;
		shape-margin: 5%;
		margin-top: 10px;
		margin-bottom: 10px;
		border-radius: 20rpx;
		width: 90%;
		height: 50rpx;
		top: 200rpx;
	}

	.search_box .search_icon {
		margin-left: 20rpx;
		margin-top: 10rpx;
		width: 30rpx;
		height: 30rpx;
	}

	.search_box .search_area {
		margin-left: 10rpx;
		margin-top: 5rpx;
		width: 80%;
		height: 40rpx;
		font-size: 30rpx;
	}

	.search_box .search_cancel {
		margin-left: 20rpx;
		margin-top: 13rpx;
		width: 30rpx;
		height: 30rpx;
	}

	.banner_box {
		margin-left: 5%;
		margin-right: 5%;
	}

	.banner_box swiper swiper-item image {
		width: 100%;
		height: 100%;
	}

	.state_box {
		width: 90%;
		margin-left: 5%;
		margin-right: 5%;
		margin-top: 40rpx;
		padding-left: 10rpx;
		padding-top: 10rpx;
		padding-bottom: 10rpx;
		border-radius: 10rpx;
		background-color: #F5F5F5;
		box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
	}

	.state_box .state_title {
		font-size: 45rpx;
		font-weight: bold;
		/* text-shadow: 1px 1px 4px #000000; */
	}

	.state_box .state_author {
		display: flex;
		margin-top: 10rpx;
		margin-left: 10rpx;
	}

	.state_box .state_author image {
		height: 45rpx;
		width: 45rpx;
		border-radius: 50%;
	}

	.state_box .state_author text {
		margin-top: 3rpx;
		margin-left: 10rpx;
		font-weight: bold;
		font-size: 30rpx;
		color: #6F6F6F;
	}

	.state_box .state_text {
		padding-left: 10rpx;
		padding-right: 10rpx;
	}

	.state_box .state_text text {
		margin-top: 20rpx;
		font-size: 35rpx;
		display: -webkit-box;
		/*弹性伸缩盒子模型显示*/
		-webkit-box-orient: vertical;
		/*排列方式*/
		-webkit-line-clamp: 8;
		/*显示文本行数*/
		overflow: hidden;
		/*溢出隐藏*/
	}

	.state_box .state_bottom {
		padding-top: 20rpx;
		position: relative;
	}

	.state_box .state_bottom .state_date {
		color: #A8A8A8;
		font-size: 30rpx;
		width: 80%;
		position: relative;
		display: inline-block;
	}

	.state_box .state_bottom .state_icon {
		margin-bottom: 10rpx;
		margin-right: 10rpx;
		height: 30rpx;
		position: absolute;
		bottom: 0;
		right: 0;
	}

	.state_box .state_bottom .state_icon image {
		margin-left: 15rpx;
		width: 30rpx;
		height: 30rpx;
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
