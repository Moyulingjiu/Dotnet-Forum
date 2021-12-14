<template>
	<view>
		<view class="search_box">
			<image class="search_icon" src="../../static/search.png"></image>
			<input class="search_area"
				 type="text"
				 v-model="searchCondition"
				 confirm-type="search"
				 @confirm="search()"
				 placeholder="搜索用户状态等"/>
		</view>

		<view class="banner_box">
			<swiper class="swiper" 
				indicator-dots="true" 
				autoplay="true" 
				interval="5000" 
				duration="1500"	>
				<swiper-item v-for="(item , index) in banner" :key="index">
					<image :src="item.img" mode="aspectFill"></image>
				</swiper-item>
			</swiper>
		</view>
		
		<view class="state_box" v-for="(item,index) in stateList">
			<view class="state_title">
				<text>{{ item.title }}</text>
			</view>
			<view class="state_text">
				<text>{{ item.text }}</text>
			</view>
			<view class="state_bottom">
				<text class="state_date">{{ item.gmtCreate }}</text>
				<view class="state_icon">
					<image src="../../static/comment.png"></image>
					<image :src="item.like?likeIcon:unlikeIcon" @click="like(index)"></image>
				</view>
			</view>
		</view>
			
		<view class="bottom_tips">
			<text>{{ (page===total-1)?bottomTipsNoMore:bottomTips }}</text>
		</view>
	</view>
</template>

<script>
	import * as userApi from "../../api/user.js"
	import * as stateApi from "../../api/state.js"
	import * as config from "../../utils/config.js"

	export default {
		data() {
			return {
				likeIcon: '../../static/like_active.png', // 点赞图标
				unlikeIcon: '../../static/like.png', // 未点赞图标
				searchCondition: '', // 搜索条件
				token: null, // token
				bottomTips: '- 上拉加载更多 -', // 底部提示
				bottomTipsNoMore: '- 到底了 -', // 底部提示
				page: 0, // 当前页码
				total: 1, // 最后一页
				banner: [
					{
						img: '../../static/banner1.jpg',
						comment: '测试备注消息1'
					},
					{
						img: '../../static/banner2.jpg',
						comment: '测试备注消息2'
					}
				], // 主页轮播图
				bannerCurrent: 0,
				bannerMode: 'round',
				stateList: [{
					id: 1,
					title: '测试状态标题1',
					userId: 1, // 用户id
					text: '测试的正文1',
					share: true,
					gmtCreate: '2021年7月20日 20:00',
					like: true
				},
				{
					id: 2,
					title: '测试状态标题2',
					userId: 2, // 用户id
					text: '从小丘西行百二十步，隔篁竹，闻水声，如鸣珮环，心乐之。伐竹取道，下见小潭，水尤清冽。全石以为底，近岸，卷石底以出，为坻，为屿，为嵁，为岩。青树翠蔓，蒙络摇缀，参差披拂。(珮 通：佩)\n潭中鱼可百许头，皆若空游无所依。日光下澈，影布石上，佁然不动；俶尔远逝，往来翕忽。似与游者相乐。(下澈 一作：下彻)\n潭西南而望，斗折蛇行，明灭可见。其岸势犬牙差互，不可知其源。\n坐潭上，四面竹树环合，寂寥无人，凄神寒骨，悄怆幽邃。以其境过清，不可久居，乃记之而去。\n同游者：吴武陵，龚古，余弟宗玄。隶而从者，崔氏二小生，曰恕己，曰奉壹。',
					share: true,
					gmtCreate: '2021年7月20日 20:00',
					like: false
				}]
			}
		},
		onLoad() {
			this.token = config.getToken()
			if (this.token === null) {
				this.login()
			} else {
				// 获取状态列表
				// this.stateList = stateApi.getStateById(this.token, this.page) 
			}
		},
		methods: {
			login() {
				// 跳转到登陆页面
				console.log('模拟登陆')
				this.token = '123456'
			},
			search() {
				// 搜索
			},
			like(index) {
				console.log('被点击')
				this.stateList.like = !this.stateList.like
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
		font-size: 40rpx;
		font-weight: bold;
		/* text-shadow: 1px 1px 4px #000000; */
	}
	
	.state_box .state_text {
		padding-left: 10rpx;
		padding-right: 10rpx;
	}
	
	.state_box .state_text text {
		font-size: 25rpx;
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
