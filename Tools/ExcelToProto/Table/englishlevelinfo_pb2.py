# Generated by the protocol buffer compiler.  DO NOT EDIT!

from google.protobuf import descriptor
from google.protobuf import message
from google.protobuf import reflection
from google.protobuf import service
from google.protobuf import service_reflection
from google.protobuf import descriptor_pb2



_ENGLISHLEVELINFO = descriptor.Descriptor(
  name='EnglishLevelInfo',
  full_name='TableProto.EnglishLevelInfo',
  filename='englishlevelinfo.proto',
  containing_type=None,
  fields=[
    descriptor.FieldDescriptor(
      name='id', full_name='TableProto.EnglishLevelInfo.id', index=0,
      number=1, type=13, cpp_type=3, label=2,
      default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    descriptor.FieldDescriptor(
      name='name', full_name='TableProto.EnglishLevelInfo.name', index=1,
      number=2, type=9, cpp_type=9, label=1,
      default_value=unicode("", "utf-8"),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    descriptor.FieldDescriptor(
      name='play_mode', full_name='TableProto.EnglishLevelInfo.play_mode', index=2,
      number=3, type=5, cpp_type=1, label=1,
      default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    descriptor.FieldDescriptor(
      name='game_flow_1', full_name='TableProto.EnglishLevelInfo.game_flow_1', index=3,
      number=4, type=13, cpp_type=3, label=3,
      default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    descriptor.FieldDescriptor(
      name='game_flow_2', full_name='TableProto.EnglishLevelInfo.game_flow_2', index=4,
      number=5, type=13, cpp_type=3, label=3,
      default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    descriptor.FieldDescriptor(
      name='game_flow_3', full_name='TableProto.EnglishLevelInfo.game_flow_3', index=5,
      number=6, type=13, cpp_type=3, label=3,
      default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    descriptor.FieldDescriptor(
      name='game_flow_4', full_name='TableProto.EnglishLevelInfo.game_flow_4', index=6,
      number=7, type=13, cpp_type=3, label=3,
      default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
    descriptor.FieldDescriptor(
      name='word', full_name='TableProto.EnglishLevelInfo.word', index=7,
      number=8, type=9, cpp_type=9, label=1,
      default_value=unicode("", "utf-8"),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],  # TODO(robinson): Implement.
  enum_types=[
  ],
  options=None)


_ENGLISHLEVELINFO_ARRAY = descriptor.Descriptor(
  name='EnglishLevelInfo_ARRAY',
  full_name='TableProto.EnglishLevelInfo_ARRAY',
  filename='englishlevelinfo.proto',
  containing_type=None,
  fields=[
    descriptor.FieldDescriptor(
      name='items', full_name='TableProto.EnglishLevelInfo_ARRAY.items', index=0,
      number=1, type=11, cpp_type=10, label=3,
      default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      options=None),
  ],
  extensions=[
  ],
  nested_types=[],  # TODO(robinson): Implement.
  enum_types=[
  ],
  options=None)


_ENGLISHLEVELINFO_ARRAY.fields_by_name['items'].message_type = _ENGLISHLEVELINFO

class EnglishLevelInfo(message.Message):
  __metaclass__ = reflection.GeneratedProtocolMessageType
  DESCRIPTOR = _ENGLISHLEVELINFO

class EnglishLevelInfo_ARRAY(message.Message):
  __metaclass__ = reflection.GeneratedProtocolMessageType
  DESCRIPTOR = _ENGLISHLEVELINFO_ARRAY

